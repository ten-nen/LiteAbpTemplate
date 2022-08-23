using LiteAbp.Application;
using LiteAbp.Application.Dtos;
using LiteAbp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LiteAbp.Api.Controllers.Backstage
{
    public class UserController : BackstageControllerBase
    {
        protected IUserService UserService { get; }
        protected IPermissionService PermissionService { get; }

        public UserController(IUserService identityUserService, IPermissionService permissionService)
        {
            UserService = identityUserService;
            PermissionService = permissionService;
        }

        [HttpGet]
        public async Task<PagedResultDto<UserDto>> GetPagerAsync(UserPagerQueryDto input)
        {
            return await UserService.GetPagerAsync(input);
        }

        [HttpPost]
        public async Task<UserDto> CreateAsync([FromBody] UserCreateDto input)
        {
            return await UserService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UserUpdateDto input)
        {
            await UserService.UpdateAsync(id, input);
        }

        [HttpGet]
        [Route("Permissions")]
        [Authorize]
        public async Task<List<PermissionInfoDto>> GetCurrentPermissionsAsync()
        {
            if (CurrentUser.Roles == null || CurrentUser.Roles.Length <= 0)
                return new List<PermissionInfoDto>();
            if (CurrentUser.Roles.Any(x => x == "admin"))
            {
                var all = PermissionService.GetAll();
                Action<List<PermissionDto>, List<PermissionInfoDto>> mapperToInfoDtos = null;
                mapperToInfoDtos = (sourceList, toList) =>
                {
                    foreach (var item in sourceList)
                    {
                        if (!toList.Any(x => x.Name == item.Name))
                            toList.Add(new PermissionInfoDto() { Name = item.Name });
                        if (item.Permissions != null)
                            mapperToInfoDtos(item.Permissions, toList);
                    }
                };
                var list = new List<PermissionInfoDto>();
                mapperToInfoDtos(all, list);
                return list;
            }
            return await UserService.GetPermissionsAsync(CurrentUser.Id.Value);
        }

        [HttpGet]
        [Route("{id}/Roles")]
        public async Task<List<RoleDto>> GetRolesAsync([FromRoute] Guid id)
        {
            return await UserService.GetRolesAsync(id);
        }

        [HttpPut]
        [Route("{id}/Roles")]
        public async Task SetRolesAsync([FromRoute] Guid id, [FromBody] UserUpdateRolesDto input)
        {
            await UserService.SetRolesAsync(id, input);
        }
    }
}
