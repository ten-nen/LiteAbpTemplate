using LiteAbp.Application.Dtos;
using LiteAbp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using LiteAbp.Application;

namespace LiteAbp.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Permissions.Users.Default)]
    public class UserController : AbpControllerBase
    {
        protected IUserService UserService { get; }
        protected IPermissionService PermissionService { get; }

        public UserController(IUserService identityUserService, IPermissionService permissionService)
        {
            UserService = identityUserService;
            PermissionService = permissionService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<UserDto> LoginAsync([FromBody] UserLoginDto user)
        {
            return await UserService.LoginAsync(user.UserName, user.Password);
        }

        [HttpGet]
        public async Task<PagedResultDto<UserDto>> GetAsync(UserPagerQueryDto input)
        {
            return await UserService.GetListAsync(input);
        }

        [HttpPost]
        [Authorize(Permissions.Users.Create)]
        public async Task<UserDto> CreateAsync([FromBody] UserCreateDto input)
        {
            return await UserService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Permissions.Users.Update)]
        public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UserUpdateDto input)
        {
            await UserService.UpdateAsync(id, input);
        }

        [HttpGet]
        [Route("{id}/Roles")]
        public async Task<List<RoleDto>> GetRolesAsync([FromRoute] Guid id)
        {
            return await UserService.GetRolesAsync(id);
        }

        [HttpPut]
        [Route("{id}/Roles")]
        [Authorize(Permissions.Users.SetRoles)]
        public async Task SetRolesAsync([FromRoute] Guid id, [FromBody] UserUpdateRolesDto input)
        {
            await UserService.SetRolesAsync(id, input);
        }
    }
}
