using LiteAbp.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace LiteAbp.Infrastructure.EfCoreRepositories
{
    public class EfCoreIdentityUserProRepository : EfCoreIdentityUserRepository, IIdentityUserProRepository
    {
        public EfCoreIdentityUserProRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider)
        : base(dbContextProvider)
        {
        }
    }
}
