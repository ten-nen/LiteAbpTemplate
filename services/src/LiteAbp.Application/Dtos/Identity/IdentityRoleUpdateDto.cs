using Volo.Abp.Domain.Entities;

namespace LiteAbp.Application.Dtos.Identity
{
    public class IdentityRoleUpdateDto : IdentityRoleCreateOrUpdateDtoBase, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; }
    }

}