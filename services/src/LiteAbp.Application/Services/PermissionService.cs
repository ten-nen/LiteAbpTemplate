using LiteAbp.Application.Dtos.Permission;
using LiteAbp.Application.Interfaces;
using LiteAbp.Extensions.Abp.Authorization.Permissions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;

namespace LiteAbp.Application.Services
{
    public class PermissionService : ApplicationService, IPermissionService
    {
        protected AppManagers AppManagers { get; }
        protected PermissionManagementOptions Options { get; }

        public PermissionService(
            AppManagers  appManagers,
            IOptions<PermissionManagementOptions> options)
        {
            Options = options.Value;
            AppManagers = appManagers;
        }
        public virtual async Task<List<PermissionGroupDto>> GetAllListAsync()
        {
            var groups = new List<PermissionGroupDto>();


            foreach (var group in AppManagers.PermissionDefinitionManager.GetGroups())
            {
                var groupDto = new PermissionGroupDto
                {
                    Name = group.Name,
                    Permissions = new List<PermissionInfoDto>()
                };
                var allPermissions = group.GetPermissionsWithChildren();
                groupDto.Permissions = allPermissions
                                            .Where(x => x.Providers.Contains(RolePathPermissionValueProvider.ProviderName) && x.Name != group.Name && x.Parent?.Name == null)
                                            .Select(x => new PermissionInfoDto()
                                            {
                                                Name = x.Name,
                                                ParentName = groupDto.Name,
                                                Permissions = allPermissions.Where(c => c.Providers.Contains(RolePathPermissionValueProvider.ProviderName) && c.Parent?.Name == x.Name)
                                                                                    .Select(c => new PermissionInfoDto() { Name = c.Name, ParentName = x.Name, Permissions = new List<PermissionInfoDto>() })
                                                                                    .ToList()
                                            })
                                            .ToList();

                groups.Add(groupDto);
            }
            return groups;
        }
    }
}
