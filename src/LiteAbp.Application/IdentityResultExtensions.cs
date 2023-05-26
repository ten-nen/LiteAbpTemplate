using Microsoft.AspNetCore.Identity;
using Volo.Abp.Identity;

namespace LiteAbp.Application
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
