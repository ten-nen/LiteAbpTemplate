using LiteAbp.Application.Dtos.Identity;
using LiteAbp.Application.Dtos.Permission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LiteAbp.Application.Interfaces
{
    public interface IIdentityRoleService : IApplicationService
    {
        Task<List<IdentityRoleDto>> GetAllListAsync();
        Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input);
        Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input);
        Task DeleteAsync(Guid id);
        Task<List<PermissionInfoDto>> GetPermissions(string roleId);
        Task UpdatePermissions(string roleId, List<IdentityRolePermissionsDtoBase> input);
    }
}
