﻿using LiteAbp.Application;
using LiteAbp.Application.Dtos.Identity;
using LiteAbp.Application.Dtos.Permission;
using LiteAbp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LiteAbp.API.Controllers.Backstage
{
    public class UserController : BackstageControllerBase
    {
        protected AppServices AppServices { get; }

        public UserController(AppServices appServices)
        {
            AppServices = appServices;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IdentityUserDto> GetAsync(Guid id)
        {
            return await AppServices.UserService.GetAsync(id);
        }

        [HttpGet]
        public async Task<PagedResultDto<IdentityUserDto>> GetPagerList(GetIdentityUsersInput input)
        {
            return await AppServices.UserService.GetPagerList(input);
        }

        [HttpPost]
        public async Task<IdentityUserDto> CreateAsync([FromBody] IdentityUserCreateDto input)
        {
            return await AppServices.UserService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task UpdateAsync(Guid id, [FromBody] IdentityUserUpdateDto input)
        {
            await AppServices.UserService.UpdateAsync(id, input);
        }

        [HttpGet]
        [Route("Permissions")]
        public async Task<List<PermissionGrantInfoDto>> GetCurrentPermissionsAsync()
        {
            if (CurrentUser.Roles == null || CurrentUser.Roles.Length <= 0)
                return new List<PermissionGrantInfoDto>();
            return await AppServices.UserService.GetPermissionsAsync(CurrentUser.Id.Value);
        }

        [HttpGet]
        [Route("{id}/Roles")]
        public async Task<List<IdentityRoleDto>> GetRolesAsync(Guid id)
        {
            return await AppServices.UserService.GetRolesAsync(id);
        }

        [HttpPut]
        [Route("{id}/Roles")]
        public async Task SetRolesAsync(Guid id,[FromBody] IdentityUserUpdateRolesDto input)
        {
            await AppServices.UserService.SetRolesAsync(id, input);
        }
    }
}