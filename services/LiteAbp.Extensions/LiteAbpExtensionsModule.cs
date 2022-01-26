using LiteAbp.Extensions.Abp.Authorization.Permissions;
using LiteAbp.Extensions.Serializations;
using LiteAbp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Threading;

namespace LiteAbp.Extensions
{
    [DependsOn(
        typeof(LiteAbpSharedModule),
        typeof(AbpAspNetCoreModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpPermissionManagementDomainModule)
        )]
    public class LiteAbpExtensionsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            //配置route小写
            Configure<RouteOptions>(options => options.LowercaseUrls = true);

            //扩展鉴权
            Configure<AbpPermissionOptions>(options => options.ValueProviders.Add<RolePathPermissionValueProvider>());

            //配置JSON时间格式
            Configure<JsonOptions>(options => options.JsonSerializerOptions.Converters.Add(new DateTimeConverter()));

        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnPreApplicationInitialization(context);

            //移除Abp默认接口
            var applicationPartManager = context.ServiceProvider.GetRequiredService<ApplicationPartManager>();
            applicationPartManager.ApplicationParts.RemoveAll(s => s.Name == "Volo.Abp.AspNetCore.Mvc");
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedDataAsync(context);
        }

        private void SeedDataAsync(ApplicationInitializationContext context)
        {
            AsyncHelper.RunSync(async () =>
            {
                using (var scope = context.ServiceProvider.CreateScope())
                {
                    await scope.ServiceProvider
                        .GetRequiredService<IDataSeeder>()
                        .SeedAsync(new DataSeedContext(null)
                             .WithProperty(IdentityDataSeedContributor.AdminEmailPropertyName, IdentityDataSeedContributor.AdminEmailDefaultValue)
                             .WithProperty(IdentityDataSeedContributor.AdminPasswordPropertyName, IdentityDataSeedContributor.AdminPasswordDefaultValue));
                }
            });
        }
    }
}
