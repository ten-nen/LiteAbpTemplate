using System.Collections.Generic;

namespace LiteAbp.Application.Dtos.Permission
{
    public class GetPermissionListResultDto
    {
        public string EntityDisplayName { get; set; }

        public List<PermissionGroupDto> Groups { get; set; }
    }
}
