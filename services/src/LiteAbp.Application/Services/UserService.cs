using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
using LiteAbp.Application.Dtos;
using System.Linq;
using LiteAbp.Extensions.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.PermissionManagement;
using LiteAbp.Domain.IRepositories;

namespace LiteAbp.Application.Services
{
    public class UserService : ApplicationService, IUserService
    {
        protected IOptions<IdentityOptions> IdentityOptions { get; }
        protected IConfiguration Configuration { get; }
        protected IdentityUserManager UserManager { get; }
        protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
        protected IUserRepository UserRepository { get; }
        protected IRoleRepository RoleRepository { get; }
        protected IPermissionRepository PermissionRepository { get; }

        public UserService(
            IOptions<IdentityOptions> identityOptions,
            IConfiguration configuration,
            IdentityUserManager userManager,
            IdentitySecurityLogManager securityLogManager,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository)
        {
            IdentityOptions = identityOptions;
            Configuration = configuration;
            UserManager = userManager;
            IdentitySecurityLogManager = securityLogManager;
            UserRepository = userRepository;
            RoleRepository = roleRepository;
            PermissionRepository = permissionRepository;
        }

        public virtual async Task<UserDto> LoginAsync(string username, string password)
        {
            var user = await UserManager.FindByNameAsync(username);

            var checkPass = await UserManager.CheckPasswordAsync(user, password);
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = "login",
                UserName = username
            });
            if (!checkPass)
            {
                throw new UserFriendlyException("登录失败", details: "账号或密码错误");
            }
            var secretKey = Configuration["App:Identity:JwtSecretKey"];
            var token = await UserManager.GenerateAuthenticationTokenAsync(user, secretKey);
            await UserManager.SetAuthenticationTokenAsync(user, "Bearer", "access_token", token);
            var userDto = ObjectMapper.Map<IdentityUser, UserDto>(user);
            userDto.Token = token;
            return userDto;
        }

        public virtual async Task<UserDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<IdentityUser, UserDto>(
                       await UserManager.GetByIdAsync(id)
                   );
        }

        public virtual async Task<PagedResultDto<UserDto>> GetPagerAsync(UserPagerQueryDto input)
        {
            Guid? roleId = null;
            if (!input.RoleName.IsNullOrWhiteSpace())
            {
                var role = await RoleRepository.FindByNormalizedNameAsync(input.RoleName);
                if (role == null)
                {
                    return new PagedResultDto<UserDto>(0, new List<UserDto>());
                }

                roleId = role.Id;
            }
            var count = await UserRepository.GetCountAsync(input.Filter, roleId);
            var list = await UserRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter, false, roleId);

            return new PagedResultDto<UserDto>(
                count,
                ObjectMapper.Map<List<IdentityUser>, List<UserDto>>(list)
            );
        }

        public virtual async Task<UserDto> CreateAsync(UserCreateDto input)
        {
            await IdentityOptions.SetAsync();

            var user = new IdentityUser(
                GuidGenerator.Create(),
                input.UserName,
                input.Email,
                CurrentTenant.Id
            );

            ObjectMapper.Map(input, user);

            (await UserManager.CreateAsync(user, input.Password)).DefaultSucceededCheck();

            (await UserManager.UpdateAsync(user)).DefaultSucceededCheck();

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, UserDto>(user);
        }

        public virtual async Task UpdateAsync(Guid id, UserUpdateDto input)
        {
            var user = await UserManager.FindByIdAsync(id.ToString());
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
                (await UserManager.SetRolesAsync(user, input.RoleNames)).DefaultSucceededCheck();
            }
            if (input.IsActive.HasValue)
            {
                user.SetIsActive(input.IsActive.Value);
            }

            (await UserManager.UpdateAsync(user)).DefaultSucceededCheck();

            if (!input.Password.IsNullOrEmpty())
            {
                (await UserManager.RemovePasswordAsync(user)).DefaultSucceededCheck();
                (await UserManager.AddPasswordAsync(user, input.Password)).DefaultSucceededCheck();
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public virtual async Task<List<RoleDto>> GetRolesAsync(Guid id)
        {
            var user = await UserManager.FindByIdAsync(id.ToString());
            if (user == null || user.Roles == null || user.Roles.Count <= 0)
                return new List<RoleDto>();
            var roles = await RoleRepository.GetLisByIdsAsync(user.Roles.Select(x => x.RoleId).ToArray());
            return ObjectMapper.Map<List<IdentityRole>, List<RoleDto>>(roles);
        }

        public virtual async Task SetRolesAsync(Guid id, UserUpdateRolesDto input)
        {
            var user = await UserManager.FindByIdAsync(id.ToString());
            if (user == null)
                throw new UserFriendlyException("用户数据不存在");

            (await UserManager.SetRolesAsync(user, input.RoleNames)).DefaultSucceededCheck();

            await UserRepository.UpdateAsync(user);
        }

        public virtual async Task<List<PermissionInfoDto>> GetPermissionsAsync(Guid id)
        {
            var user = await UserManager.FindByIdAsync(id.ToString());
            if (user == null || user.Roles == null || user.Roles.Count <= 0)
                return new List<PermissionInfoDto>();
            var list = await PermissionRepository.GetListAsync(RolePathPermissionValueProvider.ProviderName, user.Roles.Select(x => x.RoleId.ToString()).ToArray());
            return ObjectMapper.Map<List<PermissionGrant>, List<PermissionInfoDto>>(list);
        }
    }
}