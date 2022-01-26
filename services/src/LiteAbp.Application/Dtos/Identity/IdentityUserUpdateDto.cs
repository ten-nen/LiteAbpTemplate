using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LiteAbp.Application.Dtos.Identity
{
    public class IdentityUserUpdateDto : IdentityUserCreateOrUpdateDtoBase, IHasConcurrencyStamp
    {
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        public string UserName { get; set; }
        [DisableAuditing]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        public string Password { get; set; }

        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string Email { get; set; }

        public bool? IsActive { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}