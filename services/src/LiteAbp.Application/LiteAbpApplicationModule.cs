using LiteAbp.Domain;
using LiteAbp.Extensions;
using System;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace LiteAbp.Application
{
    [DependsOn(
        typeof(LiteAbpExtensionsModule),
        typeof(LiteAbpDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpPermissionManagementDomainModule),
        typeof(AbpSettingManagementDomainModule)
        )]
    public class LiteAbpApplicationModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<LiteAbpApplicationModule>();
            });
        }
    }
}
