using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement;

namespace LiteAbp.Domain
{
    [DependsOn(
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpSettingManagementDomainModule)
    )]
    public class LiteAbpDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //实体历史记录
            Configure<AbpAuditingOptions>(options =>
            {
                options.EntityHistorySelectors.Add(
                    new NamedTypeSelector(
                        "HistoryRecordedEntity",
                        type => typeof(IHistoryRecordedEntity).IsAssignableFrom(type)
                    )
                );
            });
        }
    }
}
