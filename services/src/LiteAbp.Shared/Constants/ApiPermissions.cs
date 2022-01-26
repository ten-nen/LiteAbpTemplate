using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Reflection;

namespace LiteAbp.Shared.Constants
{
    public static class ApiPermissions
    {
        public const string Backstage = "Backstage";
        public const string Forestage = "Forestage";
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(ApiPermissions));
        }
    }
}
