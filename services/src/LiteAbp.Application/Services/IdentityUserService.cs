using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiteAbp.Application.Dtos.Identity;
using LiteAbp.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using LiteAbp.Extensions.Abp.AspNetCore.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using IdentityRole = Volo.Abp.Identity.IdentityRole;
using Microsoft.Extensions.Configuration;
using LiteAbp.Extensions.Abp.Identity;
using LiteAbp.Application.Dtos.Permission;
using System.Linq;
using LiteAbp.Extensions.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.PermissionManagement;

namespace LiteAbp.Application.Services
{
    public class IdentityUserService : ApplicationService, IIdentityUserService
    {
        protected AppManagers AppManagers { get; }
        protected AppRepositories AppRepositories { get; }
        protected IOptions<IdentityOptions> IdentityOptions { get; }
        protected IConfiguration Configuration { get; }

        public IdentityUserService(
            AppManagers appManagers,
            AppRepositories appRepositories,
            IOptions<IdentityOptions> identityOptions,
            IConfiguration configuration)
        {
            AppManagers = appManagers;
            AppRepositories = appRepositories;
            IdentityOptions = identityOptions;
            Configuration = configuration;
        }

        public virtual async Task<(IdentityUserDto, string)> LoginAsync(string username, string password, bool remeberme = false)
        {
            var user = await AppManagers.UserManager.FindByNameAsync(username);

            var signInResult = await AppManagers.SignInManager.CheckPasswordSignInAsync(user, password, remeberme);
            await AppManagers.IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = "login",
                UserName = username
            });
            if (signInResult.IsLockedOut || signInResult.IsNotAllowed)
            {
                throw new UserFriendlyException("账户已锁定");
            }


            if (!signInResult.Succeeded)
            {
                throw new UserFriendlyException("登录失败", details: "账号或密码错误");
            }
            var secretKey = Configuration["App:Identity:JwtSecretKey"];
            var token = await AppManagers.UserManager.GenerateAuthenticationTokenAsync(user, secretKey);
            await AppManagers.UserManager.SetAuthenticationTokenAsync(user, "Bearer", "access_token", token);
            return (ObjectMapper.Map<IdentityUser, IdentityUserDto>(user), token);
        }

        public virtual async Task LogoutAsync(string username)
        {
            await AppManagers.IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.Logout
            });
            var user = await AppManagers.UserManager.FindByNameAsync(username);
            await AppManagers.UserManager.RemoveAuthenticationTokenAsync(user, "Bearer", "access_token");
        }

        public virtual async Task<IdentityUserDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
                       await AppManagers.UserManager.GetByIdAsync(id)
                   );
        }

        public virtual async Task<PagedResultDto<IdentityUserDto>> GetPagerList(GetIdentityUsersInput input)
        {
            Guid? roleId = null;
            if (!input.RoleName.IsNullOrWhiteSpace())
            {
                var role = await AppRepositories.RoleRepository.FindByNormalizedNameAsync(input.RoleName);
                if (role == null)
                {
                    return new PagedResultDto<IdentityUserDto>(0, new List<IdentityUserDto>());
                }

                roleId = role.Id;
            }
            var count = await AppRepositories.UserRepository.GetCountAsync(input.Filter, roleId);
            var list = await AppRepositories.UserRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter, false, roleId);

            return new PagedResultDto<IdentityUserDto>(
                count,
                ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list)
            );
        }

        public virtual async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            await IdentityOptions.SetAsync();

            var user = new IdentityUser(
                GuidGenerator.Create(),
                input.UserName,
                input.Email,
                CurrentTenant.Id
            );

            ObjectMapper.Map(input, user);

            (await AppManagers.UserManager.CreateAsync(user, input.Password)).DefaultSucceededCheck();

            (await AppManagers.UserManager.UpdateAsync(user)).DefaultSucceededCheck();

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        public virtual async Task UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            var user = await AppManagers.UserManager.FindByIdAsync(id.ToString());
            if (user == null)
                throw new UserFriendlyException("用户不存在");

            await IdentityOptions.SetAsync();

            if (input.Email != null && !string.Equals(input.Email, user.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                user.SetPhoneNumber(input.Email, false);
            }

            if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
            {
                user.SetPhoneNumber(input.PhoneNumber, false);
            }

            if (input.Name != null && !string.Equals(input.Name, user.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                user.Name = input.Name;
            }

            if (input.RoleNames != null)
            {
                (await AppManagers.UserManager.SetRolesAsync(user, input.RoleNames)).DefaultSucceededCheck();
            }
            if (input.IsActive.HasValue)
            {
                user.SetIsActive(input.IsActive.Value);
            }

            (await AppManagers.UserManager.UpdateAsync(user)).DefaultSucceededCheck();

            if (!input.Password.IsNullOrEmpty())
            {
                (await AppManagers.UserManager.RemovePasswordAsync(user)).DefaultSucceededCheck();
                (await AppManagers.UserManager.AddPasswordAsync(user, input.Password)).DefaultSucceededCheck();
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public virtual async Task<List<IdentityRoleDto>> GetRolesAsync(Guid id)
        {
            var user = await AppManagers.UserManager.FindByIdAsync(id.ToString());
            if (user == null || user.Roles == null || user.Roles.Count <= 0)
                return new List<IdentityRoleDto>();
            var roles = await AppRepositories.RoleRepository.GetLisByIdsAsync(user.Roles.Select(x => x.RoleId).ToArray());
            return ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(roles);
        }

        public virtual async Task SetRolesAsync(Guid id,IdentityUserUpdateRolesDto input)
        {
            var user = await AppManagers.UserManager.FindByIdAsync(id.ToString());
            if (user == null)
                throw new UserFriendlyException("用户数据不存在");

            (await AppManagers.UserManager.SetRolesAsync(user, input.RoleNames)).DefaultSucceededCheck();

            await AppRepositories.UserRepository.UpdateAsync(user);
        }

        public virtual async Task<List<PermissionGrantInfoDto>> GetPermissionsAsync(Guid id)
        {
            var user = await AppManagers.UserManager.FindByIdAsync(id.ToString());
            if (user == null || user.Roles == null || user.Roles.Count <= 0)
                return new List<PermissionGrantInfoDto>();
            var list = await AppRepositories.PermissionGrantRepository.GetListAsync(RolePathPermissionValueProvider.ProviderName, user.Roles.Select(x => x.RoleId.ToString()).ToArray());
            return ObjectMapper.Map<List<PermissionGrant>, List<PermissionGrantInfoDto>>(list);
        }
    }
}