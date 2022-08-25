using LiteAbp.Application;
using LiteAbp.Application.Dtos;
using LiteAbp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LiteAbp.Api.Controllers.Backstage
{
    public class RoleController : BackstageControllerBase
    {
        protected IRoleService RoleService { get; }

        public RoleController(IRoleService  identityRoleService)
        {
            RoleService = identityRoleService;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<RoleDto>> GetAllAsync()
        {
            return await RoleService.GetAllAsync();
        }

        [HttpPost]
        public async Task<RoleDto> CreateAsync([FromBody] RoleCreateDto input)
        {
            return await RoleService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<RoleDto> UpdateAsync([FromRoute] Guid id, [FromBody] RoleUpdateDto input)
        {
            return await RoleService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync([FromRoute] Guid id)
        {
            await RoleService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{roleId}/Permissions")]
        public async Task<List<PermissionDto>> GetPermissionsAsync([FromRoute] string roleId)
        {
            return await RoleService.GetPermissionsAsync(roleId);
        }

        [HttpPut]
        [Route("{roleId}/Permissions")]
        public async Task UpdatePermissionsAsync([FromRoute] string roleId, [FromBody] List<RolePermissionsDtoBase> input)
        {
            await RoleService.UpdatePermissionsAsync(roleId, input);
        }
    }
}
