using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace LiteAbp.Application.Dtos
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "用户名不能为空")]
        [StringLength(255)]
        public string UserName { get; set; }

        [Required]
        [StringLength(32)]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string Password { get; set; }
    }
    public class UserDto : ExtensibleCreationAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public bool? IsActive { get; set; }

        public string ConcurrencyStamp { get; set; }
        public string Token { get; set; }
    }

    public abstract class UserCreateOrUpdateDtoBase : ExtensibleObject
    {

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        public string Name { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        public string PhoneNumber { get; set; }

        [CanBeNull]
        public string[] RoleNames { get; set; }

        protected UserCreateOrUpdateDtoBase() : base(false)
        {

        }
    }


    public class UserPagerQueryDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string RoleName { get; set; }
    }

    public class UserCreateDto : UserCreateOrUpdateDtoBase
    {

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        public string UserName { get; set; }
        [DisableAuditing]
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string Email { get; set; }
    }

    public class UserUpdateDto : UserCreateOrUpdateDtoBase, IHasConcurrencyStamp
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


    public class UserUpdateRolesDto
    {
        [Required]
        public string[] RoleNames { get; set; }
    }
}
