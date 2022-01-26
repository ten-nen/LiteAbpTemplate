using System;
using Volo.Abp.AuditLogging;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace LiteAbp.Shared
{
    [DependsOn(
       typeof(AbpAuditLoggingDomainSharedModule),
       typeof(AbpIdentityDomainSharedModule),
       typeof(AbpPermissionManagementDomainSharedModule),
       typeof(AbpSettingManagementDomainSharedModule)
       )]
    public class LiteAbpSharedModule : AbpModule
    {
    }
}
