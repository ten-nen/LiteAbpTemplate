using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;

namespace LiteAbp.Extensions.Abp.Authorization.Permissions
{
    public class RolePathPermissionDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        protected ICurrentTenant CurrentTenant { get; }
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        protected IPermissionDataSeeder PermissionDataSeeder { get; }
        protected IIdentityRoleRepository RoleRepository { get; }

        public RolePathPermissionDataSeedContributor(
            IPermissionDefinitionManager permissionDefinitionManager,
            IPermissionDataSeeder permissionDataSeeder,
            ICurrentTenant currentTenant,
            IIdentityRoleRepository roleRepository)
        {
            PermissionDefinitionManager = permissionDefinitionManager;
            PermissionDataSeeder = permissionDataSeeder;
            CurrentTenant = currentTenant;
            RoleRepository = roleRepository;
        }

        public virtual Task SeedAsync(DataSeedContext context)
        {
            var multiTenancySide = CurrentTenant.GetMultiTenancySide();
            var permissionNames = PermissionDefinitionManager
                .GetPermissions()
                .Where(p => p.MultiTenancySide.HasFlag(multiTenancySide))
                .Where(p => !p.Providers.Any() || p.Providers.Contains(RolePathPermissionValueProvider.ProviderName))
                .Select(p => p.Name)
                .ToArray();

            var role = RoleRepository.FindByNormalizedNameAsync("ADMIN").Result;

            return PermissionDataSeeder.SeedAsync(
                    RolePathPermissionValueProvider.ProviderName,
                    role.Id.ToString(),
                    permissionNames,
                    context?.TenantId
                );
        }
    }
}
