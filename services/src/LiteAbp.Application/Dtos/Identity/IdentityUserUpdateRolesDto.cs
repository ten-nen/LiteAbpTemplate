using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteAbp.Application.Dtos.Identity
{
    public class IdentityUserUpdateRolesDto
    {
        [Required]
        public string[] RoleNames { get; set; }
    }
}
