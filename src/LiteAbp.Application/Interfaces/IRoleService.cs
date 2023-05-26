using LiteAbp.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LiteAbp.Application.Interfaces
{
    public interface IRoleService : IApplicationService
    {
        Task<List<RoleDto>> GetListAsync();
        Task<RoleDto> CreateAsync(RoleCreateDto input);
        Task<RoleDto> UpdateAsync(Guid id, RoleUpdateDto input);
        Task DeleteAsync(Guid id);
        Task<List<PermissionDto>> GetPermissionsAsync(string roleId);
        Task UpdatePermissionsAsync(string roleId, List<RolePermissionsDtoBase> input);
    }
}
