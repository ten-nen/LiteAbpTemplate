using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LiteAbp.Application.Dtos.Identity
{
    public class IdentityUserDto : ExtensibleCreationAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public bool? IsActive { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}