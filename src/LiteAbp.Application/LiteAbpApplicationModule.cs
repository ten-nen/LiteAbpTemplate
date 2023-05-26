using LiteAbp.Application.Localization;
using LiteAbp.Domain;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.VirtualFileSystem;

namespace LiteAbp.Application
{
    [DependsOn(
        typeof(LiteAbpDomainModule),
        typeof(AbpLocalizationModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpPermissionManagementDomainModule),
        typeof(AbpSettingManagementDomainModule)
        )]
    public class LiteAbpApplicationModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //配置AutoMapper
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<LiteAbpApplicationModule>();
            });

            //本地化
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<LiteAbpApplicationModule>();
            });
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AppResource>("zh-Hans")
                    .AddVirtualJson("/Localization/Resources");
            });
        }
    }
}
