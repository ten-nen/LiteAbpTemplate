using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.PermissionManagement;

namespace LiteAbp.Domain.IRepositories
{
    public interface IPermissionGrantProRepository: IPermissionGrantRepository
    {
        Task<List<PermissionGrant>> GetListAsync(
        string providerName,
        string[] providerKeys,
        CancellationToken cancellationToken = default);
    }
}
