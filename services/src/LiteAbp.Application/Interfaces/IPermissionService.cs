using LiteAbp.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LiteAbp.Application.Interfaces
{
    public interface IPermissionService: IApplicationService
    {
        List<PermissionDto> GetAll();
    }
}
