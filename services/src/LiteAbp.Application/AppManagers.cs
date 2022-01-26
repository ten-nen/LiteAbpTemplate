using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.SimpleStateChecking;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LiteAbp.Application
{
    public class AppManagers : ITransientDependency
    {
        public SignInManager<IdentityUser> SignInManager { get; }
        public IdentityUserManager UserManager { get; }
        public IdentityRoleManager RoleManager { get; }
        public IdentitySecurityLogManager IdentitySecurityLogManager { get; }
        public IPermissionDefinitionManager PermissionDefinitionManager { get; }
        public ISimpleStateCheckerManager<PermissionDefinition> SimpleStateCheckerManager { get; }
        public AppManagers(
            SignInManager<IdentityUser> signInManager,
            IdentityUserManager userManager,
            IdentityRoleManager roleManager,
            IdentitySecurityLogManager identitySecurityLogManager,
            IPermissionDefinitionManager permissionDefinitionManager,
            ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            RoleManager = roleManager;
            IdentitySecurityLogManager = identitySecurityLogManager;
            PermissionDefinitionManager = permissionDefinitionManager;
            SimpleStateCheckerManager = simpleStateCheckerManager;
        }
    }
}
