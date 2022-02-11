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
        public SignInManager<IdentityUser> SignInManager { get; set; }
        public IdentityUserManager UserManager { get; set; }
        public IdentityRoleManager RoleManager { get; set; }
        public IdentitySecurityLogManager IdentitySecurityLogManager { get; set; }
        public IPermissionDefinitionManager PermissionDefinitionManager { get; set; }
        public ISimpleStateCheckerManager<PermissionDefinition> SimpleStateCheckerManager { get; set; }
    }
}
