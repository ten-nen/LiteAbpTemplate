using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Reflection;

namespace LiteAbp.Extensions
{
    public static class ApiPermissions
    {
        public const string Backstage = "backstage";
        public const string Forestage = "forestage";
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(ApiPermissions));
        }
    }
}
