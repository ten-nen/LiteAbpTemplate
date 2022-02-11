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
                    var controllerPermission = group.AddPermission($"{p}.{controller}").WithProviders(RolePathPermissionValueProvider.ProviderName);
                    foreach (var action in policyRoutes.Where(v => v.Controller == controller).Select(v => v.Action).Distinct())
                    {
                        controllerPermission.AddChild($"{p}.{controller}.{action}").WithProviders(RolePathPermissionValueProvider.ProviderName);
                    }
                }
            }

            //var abpControllerTypes = Assembly.GetAssembly(typeof(AttributeUsageAttribute))
            //                 .GetTypes();
            //var policyControllerTypes = abpControllerTypes.Where(v => v.GetCustomAttributes<AuthorizeAttribute>(true).Any(attr => attr.Policy == p));
            //var policyControllerActions = policyControllerTypes.SelectMany(v => v.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            //                            .Where(v => !v.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
            //                            .Select(v =>
            //                            {
            //                                var controller = v.DeclaringType.Name.Replace("Controller", "");
            //                                var action = v.Name.EndsWith("Async") ? v.Name.Remove(v.Name.IndexOf("Async")) : v.Name;
            //                                return new { controller, action };
            //                            })
            //                            .ToList();
            //foreach (var controller in policyControllerActions.Select(v => v.controller).Distinct())
            //{
            //    var controllerPermission = group.AddPermission($"{p}.{controller}").WithProviders(RolePathPermissionValueProvider.ProviderName);
            //    foreach (var action in policyControllerActions.Where(v => v.controller == controller).Select(v => v.action).Distinct())
            //    {
            //        controllerPermission.AddChild($"{p}.{controller}.{action}").WithProviders(RolePathPermissionValueProvider.ProviderName);
            //    }
            //}
        }
    }
}
