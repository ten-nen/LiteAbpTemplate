using LiteAbp.Application.Dtos;
using LiteAbp.Application.Interfaces;
using LiteAbp.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using IdentityRole = Volo.Abp.Identity.IdentityRole;

namespace LiteAbp.Application.Services
{
    public class RoleService : ApplicationService, IRoleService
    {
        protected IdentityRoleManager RoleManager { get; }
        protected IRoleRepository RoleRepository { get; }
        protected IPermissionManager PermissionManager { get; }

        public RoleService(
            IdentityRoleManager roleManager,
            IRoleRepository roleRepository,
            IPermissionManager permissionManager)
        {
            RoleManager = roleManager;
            RoleRepository = roleRepository;
            PermissionManager = permissionManager;
        }
        public virtual async Task<RoleDto> CreateAsync(RoleCreateDto input)
        {
            var exsitRole = await RoleManager.FindByNameAsync(input.Name);
            if (exsitRole != null)
                throw new UserFriendlyException("角色名称已存在");

            var role = new IdentityRole(GuidGenerator.Create(), input.Name, null);

            input.MapExtraPropertiesTo(role);

            (await RoleManager.CreateAsync(role)).DefaultSucceededCheck();

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityRole, RoleDto>(role);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var role = await RoleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return;
            }

            (await RoleManager.DeleteAsync(role)).DefaultSucceededCheck();

            await PermissionManager.DeleteAsync(RolePermissionValueProvider.ProviderName, role.Name);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public virtual async Task<RoleDto> UpdateAsync(Guid id, RoleUpdateDto input)
        {
            var role = await RoleManager.GetByIdAsync(id);

            if (role.Name == input.Name)
                return ObjectMapper.Map<IdentityRole, RoleDto>(role);

            var list = await RoleRepository.GetListAsync(filter: input.Name);
            if (list.Any(x => x.Name == input.Name && x.Id != role.Id))
                throw new UserFriendlyException("角色名称已存在");

            (await RoleManager.SetRoleNameAsync(role, input.Name)).DefaultSucceededCheck();

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityRole, RoleDto>(role);
        }

        public virtual async Task<List<RoleDto>> GetListAsync()
        {
            var list = await RoleRepository.GetListAsync();
            return ObjectMapper.Map<List<IdentityRole>, List<RoleDto>>(list);
        }

        public virtual async Task<List<PermissionDto>> GetPermissionsAsync(string roleId)
        {
            var role = await RoleManager.FindByIdAsync(roleId);
            if (role == null)
                return new List<PermissionDto>();
            var list = await PermissionManager.GetAllForRoleAsync(role.Name);
            return list.Where(x => x.IsGranted).Select(x => new PermissionDto() { Name = x.Name }).ToList();
        }

        public virtual async Task UpdatePermissionsAsync(string roleId, List<RolePermissionsDtoBase> input)
        {
            var role = await RoleManager.FindByIdAsync(roleId);
            if (role == null)
                return;
            var permissions = await PermissionManager.GetAllForRoleAsync(role.Name);
            foreach (var permission in permissions)
            {
                if (input.Any(x => x.Name == permission.Name) && !permission.IsGranted)
                    await PermissionManager.SetForRoleAsync(role.Name, permission.Name, true);
                else if (!input.Any(x => x.Name == permission.Name) && permission.IsGranted)
                    await PermissionManager.SetForRoleAsync(role.Name, permission.Name, false);
            }
            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}
