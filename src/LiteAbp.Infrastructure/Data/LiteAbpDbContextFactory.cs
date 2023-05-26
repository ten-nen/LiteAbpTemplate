using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteAbp.Infrastructure.Data
{
    public class LiteAbpDbContextFactory : IDesignTimeDbContextFactory<LiteAbpDbContext>
    {
        public LiteAbpDbContext CreateDbContext(string[] args)
        {

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<LiteAbpDbContext>()
#if (mysql)
                .UseMySql(configuration.GetConnectionString("Default"), MySqlServerVersion.LatestSupportedServerVersion);
#else
                .UseSqlServer(configuration.GetConnectionString("Default"));
#endif

            return new LiteAbpDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../LiteAbp.Api/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
