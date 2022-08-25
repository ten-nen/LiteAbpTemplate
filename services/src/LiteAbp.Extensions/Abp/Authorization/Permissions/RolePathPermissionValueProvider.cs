using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;

namespace LiteAbp.Extensions.Abp.Authorization.Permissions
{
    public class RolePathPermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "RP";
        public override string Name => ProviderName;
        private IHttpContextAccessor _httpContextAccessor;
        protected IIdentityRoleRepository RoleRepository { get; }
        public RolePathPermissionValueProvider(IHttpContextAccessor httpContextAccessor, IPermissionStore permissionStore, IIdentityRoleRepository roleRepository)
        : base(permissionStore)
        {
            _httpContextAccessor = httpContextAccessor;
            RoleRepository = roleRepository;
        }

        public async override Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
        {
            var ignoreAuthorize = _httpContextAccessor.HttpContext.GetEndpoint().Metadata.Any(x => x is AuthorizeAttribute && ((AuthorizeAttribute)x).Policy == null);
            if (context.Principal != null && ignoreAuthorize)
                return PermissionGrantResult.Granted;

            var roles = context.Principal?.FindAll(AbpClaimTypes.Role).Select(c => c.Value).ToArray();

            if (roles == null || !roles.Any())
            {
                return PermissionGrantResult.Undefined;
            }

            if (roles.Any(x => x == "admin"))
            {
                return PermissionGrantResult.Granted;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            var routeData = httpContext.GetRouteData();
            var controller = routeData?.Values["controller"]?.ToString().ToLower();
            var action = routeData?.Values["action"]?.ToString().ToLower();

            var roleInfos = await RoleRepository.GetListAsync();
            foreach (var roleInfo in roleInfos.Where(x => roles.Contains(x.Name)).Distinct())
            {
                if (await PermissionStore.IsGrantedAsync($"{context.Permission.Name?.ToLower()}.{controller}.{action}", Name, roleInfo.Id.ToString()))
                {
                    return PermissionGrantResult.Granted;
                }
            }

            return PermissionGrantResult.Undefined;
        }

        public override async Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
        {
            var permissionNames = context.Permissions.Select(x => x.Name).Distinct().ToList();
            Check.NotNullOrEmpty(permissionNames, nameof(permissionNames));

            var result = new MultiplePermissionGrantResult(permissionNames.ToArray());

            var ignoreAuthorize = _httpContextAccessor.HttpContext.GetEndpoint().Metadata.Any(x => x is AuthorizeAttribute && ((AuthorizeAttribute)x).Policy == null);
            if (context.Principal != null && ignoreAuthorize)
            {
                foreach (var key in result.Result.Keys)
                {
                    result.Result[key] = PermissionGrantResult.Granted;
                }
                return result;
            }

            var roles = context.Principal?.FindAll(AbpClaimTypes.Role).Select(c => c.Value).ToArray();
            if (roles == null || !roles.Any())
            {
                return result;
            }


            if (roles.Any(x => x == "admin"))
            {
                foreach (var key in result.Result.Keys)
                {
                    result.Result[key] = PermissionGrantResult.Granted;
                }
                return result;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            var routeData = httpContext.GetRouteData();
            var controller = routeData?.Values["controller"]?.ToString().ToLower();
            var action = routeData?.Values["action"]?.ToString().ToLower();

            foreach (var role in roles.Distinct())
            {
                var multipleResult = await PermissionStore.IsGrantedAsync(permissionNames.Select(v => $"{v?.ToLower()}.{controller}.{action}").ToArray(), Name, role);

                foreach (var grantResult in multipleResult.Result.Where(grantResult =>
                    result.Result.ContainsKey(grantResult.Key) &&
                    result.Result[grantResult.Key] == PermissionGrantResult.Undefined &&
                    grantResult.Value != PermissionGrantResult.Undefined))
                {
                    result.Result[grantResult.Key] = grantResult.Value;
                    permissionNames.RemoveAll(x => x == grantResult.Key);
                }

                if (result.AllGranted || result.AllProhibited)
                {
                    break;
                }

                if (permissionNames.IsNullOrEmpty())
                {
                    break;
                }
            }

            return result;
        }
    }
}
