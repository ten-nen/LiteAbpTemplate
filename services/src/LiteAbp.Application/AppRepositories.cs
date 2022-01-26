using LiteAbp.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace LiteAbp.Application
{
    public class AppRepositories : ITransientDependency
    {
        public IIdentityUserProRepository UserRepository { get; }
        public IIdentityRoleProRepository RoleRepository { get; }
        public IPermissionGrantProRepository PermissionGrantRepository { get; }
        public AppRepositories(
            IIdentityUserProRepository userRepository,
            IIdentityRoleProRepository roleRepository,
            IPermissionGrantProRepository permissionGrantRepository)
        {

            UserRepository = userRepository;
            RoleRepository = roleRepository;
            PermissionGrantRepository = permissionGrantRepository;
        }
    }
}
