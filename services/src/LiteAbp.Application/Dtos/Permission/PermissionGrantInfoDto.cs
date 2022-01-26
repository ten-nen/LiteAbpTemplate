using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LiteAbp.Application.Dtos.Permission
{
   public class PermissionGrantInfoDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
