using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace LiteAbp.Application.Dtos.Identity
{
    public class IdentityRoleCreateOrUpdateDtoBase : ExtensibleObject
    {
        [Required]
        [DynamicStringLength(typeof(IdentityRoleConsts), nameof(IdentityRoleConsts.MaxNameLength))]
        public string Name { get; set; }

        protected IdentityRoleCreateOrUpdateDtoBase() : base(false)
        {

        }
    }
}