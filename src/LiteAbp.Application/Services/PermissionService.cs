using LiteAbp.Application.Interfaces;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.PermissionManagement;

namespace LiteAbp.Application.Services
{
    public class PermissionService : ApplicationService, IPermissionService
    {
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        protected PermissionManagementOptions Options { get; }

        public PermissionService(
            IOptions<PermissionManagementOptions> options,
            IPermissionDefinitionManager permissionDefinitionManager)
        {
            PermissionDefinitionManager = permissionDefinitionManager;
            Options = options.Value;
        }
    }
}
