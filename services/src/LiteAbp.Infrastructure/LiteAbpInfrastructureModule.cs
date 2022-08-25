using LiteAbp.Domain;
using LiteAbp.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
#if (mysql)
using Volo.Abp.EntityFrameworkCore.MySQL;
#else
using Volo.Abp.EntityFrameworkCore.SqlServer;
#endif

namespace LiteAbp.Infrastructure
{
    [DependsOn(
       typeof(LiteAbpDomainModule),
       typeof(AbpIdentityEntityFrameworkCoreModule),
       typeof(AbpPermissionManagementEntityFrameworkCoreModule),
       typeof(AbpSettingManagementEntityFrameworkCoreModule),
#if (mysql)
       typeof(AbpEntityFrameworkCoreMySQLModule),
#else
       typeof(AbpEntityFrameworkCoreSqlServerModule),
#endif
       typeof(AbpAuditLoggingEntityFrameworkCoreModule)
       )]
    public class LiteAbpInfrastructureModule:AbpModule
    {
        //todo: 1、创建数据库 dotnet ef migrations add InitialCreate -o ./Data/Migrations
        //                 dotnet ef database update
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<LiteAbpDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also demoMigrationsDbContextFactory for EF Core tooling. */
#if (mysql)
                options.UseMySQL();
#else
                options.UseSqlServer();
#endif

            });
        }
    }
}
