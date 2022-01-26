using System.Collections.Generic;

namespace LiteAbp.Application.Dtos.Permission
{
    public class PermissionGroupDto
    {
        public string Name { get; set; }

        public List<PermissionInfoDto> Permissions { get; set; }
    }
}