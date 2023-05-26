using LiteAbp.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace LiteAbp.Infrastructure.Repositories
{
    public class PermissionRepository: EfCorePermissionGrantRepository, IPermissionRepository
    {
        public PermissionRepository(IDbContextProvider<IPermissionManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
        {

        }

        public virtual async Task<List<PermissionGrant>> GetListAsync(
        string providerName,
        string[] providerKeys,
        CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(x =>
                    x.ProviderName == providerName &&
                   providerKeys.Contains( x.ProviderKey)
                ).ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
