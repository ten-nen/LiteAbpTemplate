using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace LiteAbp.API.Controllers.Models
{
    public class UserLoginInfo
    {
        [Required(ErrorMessage ="用户名不能为空")]
        [StringLength(255)]
        public string UserName { get; set; }

        [Required]
        [StringLength(32)]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
