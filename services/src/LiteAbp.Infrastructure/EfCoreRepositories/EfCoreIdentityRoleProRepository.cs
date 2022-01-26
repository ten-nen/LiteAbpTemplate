using LiteAbp.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace LiteAbp.Infrastructure.EfCoreRepositories
{
    public class EfCoreIdentityRoleProRepository : EfCoreIdentityRoleRepository, IIdentityRoleProRepository
    {
        public EfCoreIdentityRoleProRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider)
        : base(dbContextProvider)
        {
        }

        public virtual async Task<List<IdentityRole>> GetLisByIdsAsync(
        Guid[] ids,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .WhereIf(ids != null, x => ids.Contains(x.Id))
            .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
