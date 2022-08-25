using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace LiteAbp.Extensions.Abp.AspNetCore.Identity
{
    public static class IdentityResultExtensions
    {
        public static void DefaultSucceededCheck(this IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
                throw new AbpIdentityResultException(identityResult);
        }
    }
}
