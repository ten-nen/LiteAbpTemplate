using LiteAbp.Application.Dtos.Identity;
using LiteAbp.Application.Dtos.Permission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LiteAbp.Application.Interfaces
{
    public interface IIdentityUserService : IApplicationService
    {
        Task<(IdentityUserDto, string)> LoginAsync(string username, string password, bool remeberme = false);
        Task LogoutAsync(string username);
        Task<IdentityUserDto> GetAsync(Guid id);
        Task<PagedResultDto<IdentityUserDto>> GetPagerList(GetIdentityUsersInput input);
        Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input);
        Task UpdateAsync(Guid id, IdentityUserUpdateDto input);
        Task<List<IdentityRoleDto>> GetRolesAsync(Guid id);
        Task SetRolesAsync(Guid id, IdentityUserUpdateRolesDto input);
        Task<List<PermissionGrantInfoDto>> GetPermissionsAsync(Guid id);
    }
}