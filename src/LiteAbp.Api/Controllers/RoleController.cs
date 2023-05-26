using LiteAbp.Application;
using LiteAbp.Application.Dtos;
using LiteAbp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace LiteAbp.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Permissions.Roles.Default)]
    public class RoleController : AbpControllerBase
    {
        protected IRoleService RoleService { get; }

        public RoleController(IRoleService identityRoleService)
        {
            RoleService = identityRoleService;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<RoleDto>> GetListAsync()
        {
            return await RoleService.GetListAsync();
        }

        [HttpPost]
        [Authorize(Permissions.Roles.Create)]
        public async Task<RoleDto> CreateAsync([FromBody] RoleCreateDto input)
        {
            return await RoleService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Permissions.Roles.Update)]
        public async Task<RoleDto> UpdateAsync([FromRoute] Guid id, [FromBody] RoleUpdateDto input)
        {
            return await RoleService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Permissions.Roles.Delete)]
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
        [Authorize(Permissions.Roles.UpdatePermissions)]
        public async Task UpdatePermissionsAsync([FromRoute] string roleId, [FromBody] List<RolePermissionsDtoBase> input)
        {
            await RoleService.UpdatePermissionsAsync(roleId, input);
        }
    }
}
