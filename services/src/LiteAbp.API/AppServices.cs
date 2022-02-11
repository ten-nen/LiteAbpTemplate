using LiteAbp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace LiteAbp.API
{
    public class AppServices : IRemoteService, ITransientDependency
    {
        public IIdentityUserService UserService { get; set; }
        public IIdentityRoleService RoleService { get; set; }
        public IPermissionService PermissionService { get; set; }
    }
}
