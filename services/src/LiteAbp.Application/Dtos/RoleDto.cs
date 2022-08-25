using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Validation;

namespace LiteAbp.Application.Dtos
{
    public class RoleDto : ExtensibleEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Name { get; set; }
        public bool IsStatic { get; set; }

        public string ConcurrencyStamp { get; set; }
    }

    public class RoleCreateOrUpdateDtoBase : ExtensibleObject
    {
        [Required]
        [DynamicStringLength(typeof(IdentityRoleConsts), nameof(IdentityRoleConsts.MaxNameLength))]
        public string Name { get; set; }

        protected RoleCreateOrUpdateDtoBase() : base(false)
        {

        }
    }

    public class RoleCreateDto : RoleCreateOrUpdateDtoBase
    {

    }

    public class RoleUpdateDto : RoleCreateOrUpdateDtoBase, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; }
    }

    public class RolePermissionsDtoBase : ExtensibleObject
    {
        [Required]
        [DynamicStringLength(typeof(PermissionGrantConsts), nameof(PermissionGrantConsts.MaxNameLength))]
        public string Name { get; set; }
    }
}
