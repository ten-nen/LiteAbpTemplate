using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace LiteAbp.Application.Dtos.Identity
{
    public abstract class IdentityUserCreateOrUpdateDtoBase : ExtensibleObject
    {

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        public string Name { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        public string PhoneNumber { get; set; }

        [CanBeNull]
        public string[] RoleNames { get; set; }

        protected IdentityUserCreateOrUpdateDtoBase() : base(false)
        {

        }
    }
}
