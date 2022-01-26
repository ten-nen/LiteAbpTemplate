﻿using LiteAbp.Application;
using LiteAbp.Application.Dtos.Identity;
using LiteAbp.Application.Dtos.Permission;
using LiteAbp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LiteAbp.API.Controllers.Backstage
{
    public class RoleController : BackstageControllerBase
    {
        protected AppServices AppServices { get; }

        public RoleController(AppServices appServices)
        {
            AppServices = appServices;
        }

        [HttpGet]
        public async Task<List<IdentityRoleDto>> GetAllListAsync()
        {
            return await AppServices.RoleService.GetAllListAsync();
        }

        [HttpPost]
        public async Task<IdentityRoleDto> CreateAsync([FromBody] IdentityRoleCreateDto input)
        {
            return await AppServices.RoleService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IdentityRoleDto> UpdateAsync(Guid id, [FromBody] IdentityRoleUpdateDto input)
        {
            return await AppServices.RoleService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            await AppServices.RoleService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{roleId}/permissions")]
        public async Task<List<PermissionInfoDto>> GetPermissions(string roleId)
        {
            return await AppServices.RoleService.GetPermissions(roleId);
        }

        [HttpPut]
        [Route("{roleId}/permissions")]
        public async Task UpdatePermissionsAsync(string roleId, [FromBody] List<IdentityRolePermissionsDtoBase> input)
        {
            await AppServices.RoleService.UpdatePermissions(roleId, input);
        }
    }
}