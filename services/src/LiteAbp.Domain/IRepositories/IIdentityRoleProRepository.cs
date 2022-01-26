using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace LiteAbp.Domain.IRepositories
{
    public interface IIdentityRoleProRepository: IIdentityRoleRepository
    {
        Task<List<IdentityRole>> GetLisByIdsAsync(
        Guid[] ids,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);
    }
}
