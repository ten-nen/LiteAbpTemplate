using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LiteAbp.Application.Dtos
{
    public class PermissionDto
    {
        public string Name { get; set; }

        public string ParentName { get; set; }

        public List<PermissionDto> Permissions { get; set; }
    }
    public class PermissionInfoDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
