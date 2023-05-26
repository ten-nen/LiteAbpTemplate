﻿using Volo.Abp.Reflection;

namespace LiteAbp.Application
{
    public static class Permissions
    {
        public const string GroupName = "Api";
        public static class Roles
        {
            public const string Default = GroupName + ".Roles";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string UpdatePermissions = Default + ".UpdatePermissions";
        }
        public static class Users
        {
            public const string Default = GroupName + ".Users";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string SetRoles = Default + ".SetRoles";
        }
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(Permissions));
        }
    }
}
