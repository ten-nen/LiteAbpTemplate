using LiteAbp.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Volo.Abp.AuditLogging;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement;

namespace LiteAbp.Domain
{
    [DependsOn(
        typeof(LiteAbpSharedModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpSettingManagementDomainModule)
    )]
    public class LiteAbpDomainModule : AbpModule
    {
    }
}
