using Volo.Abp.Application.Dtos;

namespace LiteAbp.Application.Dtos.Identity
{
    public class GetIdentityUsersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string RoleName { get; set; }
    }
}