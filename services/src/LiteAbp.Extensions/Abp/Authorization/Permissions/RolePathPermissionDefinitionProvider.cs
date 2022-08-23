using LiteAbp.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;

namespace LiteAbp.Extensions.Abp.Authorization.Permissions
{
    public class RolePathPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        private readonly IActionDescriptorCollectionProvider _provider;
        public RolePathPermissionDefinitionProvider(IActionDescriptorCollectionProvider provider)
        {
            _provider = provider;
        }
        public override void Define(IPermissionDefinitionContext context)
        {

            var policies = ApiPermissions.GetAll();

            var routes = _provider.ActionDescriptors.Items
                                                    .Where(x=>!x.EndpointMetadata.Any(e=> e is AllowAnonymousAttribute)&& x.EndpointMetadata.Any(e => e is AuthorizeAttribute))
                                                    .Select(x => new
                                                                {
                                                                    Action = x.RouteValues["Action"],
                                                                    Controller = x.RouteValues["Controller"],
                                                                    Policy = ((AuthorizeAttribute)x.EndpointMetadata.FirstOrDefault(e=> e is AuthorizeAttribute))?.Policy
                                                                })
                                                    .ToList();

            foreach (var p in policies)
            {
                var group = context.AddGroup(p);
                group.AddPermission(p).WithProviders(RolePathPermissionValueProvider.ProviderName);

                var policyRoutes = routes.Where(v => v.Policy == p).ToList();
                foreach (var controller in policyRoutes.Select(v => v.Controller).Distinct())
                {
                    var controllerPermission = group.AddPermission($"{p}.{controller.ToLower()}").WithProviders(RolePathPermissionValueProvider.ProviderName);
                    foreach (var action in policyRoutes.Where(v => v.Controller == controller).Select(v => v.Action).Distinct())
                    {
                        controllerPermission.AddChild($"{p}.{controller.ToLower()}.{action.ToLower()}").WithProviders(RolePathPermissionValueProvider.ProviderName);
                    }
                }
            }
        }
    }
}
