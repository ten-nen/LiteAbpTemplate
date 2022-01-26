using LiteAbp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LiteAbp.API
{
    public class AppServices : IRemoteService, ITransientDependency
    {
        public IIdentityUserService UserService { get; }
        public IIdentityRoleService RoleService { get; }
        public IPermissionService PermissionService { get; }
        public AppServices(IIdentityUserService identityUserService, IIdentityRoleService identityRoleService, IPermissionService permissionService)
        {
            UserService = identityUserService;
            RoleService = identityRoleService;
            PermissionService = permissionService;
        }
    }
}
