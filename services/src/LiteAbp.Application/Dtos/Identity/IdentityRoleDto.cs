using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LiteAbp.Application.Dtos.Identity
{
    public class IdentityRoleDto : ExtensibleEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Name { get; set; }
        public bool IsStatic { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}