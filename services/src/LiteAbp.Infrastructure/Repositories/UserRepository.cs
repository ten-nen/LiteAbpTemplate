using LiteAbp.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace LiteAbp.Infrastructure.Repositories
{
    public class UserRepository : EfCoreIdentityUserRepository, IUserRepository
    {
        public UserRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider)
        : base(dbContextProvider)
        {
        }
    }
}
