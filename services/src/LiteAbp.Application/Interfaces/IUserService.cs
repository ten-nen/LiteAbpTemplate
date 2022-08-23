using LiteAbp.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LiteAbp.Application.Interfaces
{
    public interface IUserService : IApplicationService
    {
        Task<UserDto> LoginAsync(string username, string password);
        Task<UserDto> GetAsync(Guid id);
        Task<PagedResultDto<UserDto>> GetPagerAsync(UserPagerQueryDto input);
        Task<UserDto> CreateAsync(UserCreateDto input);
        Task UpdateAsync(Guid id, UserUpdateDto input);
        Task<List<RoleDto>> GetRolesAsync(Guid id);
        Task SetRolesAsync(Guid id, UserUpdateRolesDto input);
        Task<List<PermissionInfoDto>> GetPermissionsAsync(Guid id);
    }
}