using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LiteAbp.Application.Dtos.Permission
{

    public class PermissionInfoDto 
    {
        public string Name { get; set; }

        public string ParentName { get; set; }

        public List<PermissionInfoDto> Permissions { get; set; }
    }
}