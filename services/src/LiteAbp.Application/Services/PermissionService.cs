using LiteAbp.Application.Dtos;
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
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        protected PermissionManagementOptions Options { get; }

        public PermissionService(
            IOptions<PermissionManagementOptions> options,
            IPermissionDefinitionManager permissionDefinitionManager)
        {
            PermissionDefinitionManager = permissionDefinitionManager;
            Options = options.Value;
        }
        public virtual List<PermissionDto> GetAll()
        {
            var groups = new List<PermissionDto>();


            foreach (var group in PermissionDefinitionManager.GetGroups())
            {
                var groupDto = new PermissionDto
                {
                    Name = group.Name,
                    ParentName = "",
                    Permissions = new List<PermissionDto>()
                };
                var allPermissions = group.GetPermissionsWithChildren();
                groupDto.Permissions = allPermissions
                                            .Where(x => x.Providers.Contains(RolePathPermissionValueProvider.ProviderName) && x.Name != group.Name && x.Parent?.Name == null)
                                            .Select(x => new PermissionDto()
                                            {
                                                Name = x.Name,
                                                ParentName = groupDto.Name,
                                                Permissions = allPermissions.Where(c => c.Providers.Contains(RolePathPermissionValueProvider.ProviderName) && c.Parent?.Name == x.Name)
                                                                                    .Select(c => new PermissionDto() { Name = c.Name, ParentName = x.Name, Permissions = new List<PermissionDto>() })
                                                                                    .ToList()
                                            })
                                            .ToList();

                groups.Add(groupDto);
            }
            return groups;
        }
    }
}
