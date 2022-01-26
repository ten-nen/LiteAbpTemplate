using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Validation;

namespace LiteAbp.Application.Dtos.Identity
{
    public class IdentityRolePermissionsDtoBase : ExtensibleObject
    {
        [Required]
        [DynamicStringLength(typeof(PermissionGrantConsts), nameof(PermissionGrantConsts.MaxNameLength))]
        public string Name { get; set; }
    }
}
