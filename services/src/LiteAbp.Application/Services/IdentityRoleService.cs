using LiteAbp.Application.Dtos.Identity;
using LiteAbp.Application.Dtos.Permission;
using LiteAbp.Application.Interfaces;
using LiteAbp.Domain;
using LiteAbp.Extensions.Abp.AspNetCore.Identity;
using LiteAbp.Extensions.Abp.Authorization.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using IdentityRole = Volo.Abp.Identity.IdentityRole;

namespace LiteAbp.Application.Services
{
    public class IdentityRoleService : ApplicationService, IIdentityRoleService
    {
        protected AppManagers AppManagers { get; }
        protected AppRepositories AppRepositories { get; }
        protected IPermissionDataSeeder PermissionDataSeeder { get; }

        public IdentityRoleService(
            AppManagers  appManagers,
            AppRepositories  appRepositories,
            IPermissionDataSeeder permissionDataSeeder)
        {
            AppManagers = appManagers;
            AppRepositories = appRepositories;
            PermissionDataSeeder = permissionDataSeeder;
        }
        public virtual async Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input)
        {
            var exsitRole = await AppManagers.RoleManager.FindByNameAsync(input.Name);
            if (exsitRole != null)
                throw new UserFriendlyException("角色名称已存在");

            var role = new IdentityRole(GuidGenerator.Create(), input.Name, null);

            input.MapExtraPropertiesTo(role);

            (await AppManagers.RoleManager.CreateAsync(role)).DefaultSucceededCheck();

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var role = await AppManagers.RoleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return;
            }

            ( await AppManagers.RoleManager.DeleteAsync(role)).DefaultSucceededCheck();

            var rolePermissions = await AppRepositories.PermissionGrantRepository.GetListAsync(RolePathPermissionValueProvider.ProviderName, id.ToString());
            await AppRepositories.PermissionGrantRepository.DeleteManyAsync(rolePermissions.Select(x => x.Id));
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public virtual async Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input)
        {
            var role = await AppManagers.RoleManager.GetByIdAsync(id);

            if (role.Name == input.Name)
                return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role);

            var list = await AppRepositories.RoleRepository.GetListAsync(filter: input.Name);
            if (list.Any(x => x.Name == input.Name && x.Id != role.Id))
                throw new UserFriendlyException("角色名称已存在");

            ( await AppManagers.RoleManager.SetRoleNameAsync(role, input.Name)).DefaultSucceededCheck();

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role);
        }

        public virtual async Task<List<IdentityRoleDto>> GetAllListAsync()
        {
            var list = await AppRepositories.RoleRepository.GetListAsync();
            return ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list);
        }

        public virtual async Task<List<PermissionInfoDto>> GetPermissions(string roleId)
        {
            var list = await AppRepositories.PermissionGrantRepository.GetListAsync(RolePathPermissionValueProvider.ProviderName, roleId);
            return list.Select(x => new PermissionInfoDto() { Name = x.Name }).ToList();
        }

        public virtual async Task UpdatePermissions(string roleId, List<IdentityRolePermissionsDtoBase> input)
        {
            var list = await AppRepositories.PermissionGrantRepository.GetListAsync(RolePathPermissionValueProvider.ProviderName, roleId);
            await AppRepositories.PermissionGrantRepository.DeleteManyAsync(list, true);
            await PermissionDataSeeder.SeedAsync(RolePathPermissionValueProvider.ProviderName, roleId, input.Select(x => x.Name));
        }
    }
}
