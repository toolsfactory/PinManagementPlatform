using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace PinPlatform.Common
{
    public static class ApplicationBuilderDBExtensions
    {
        public static void EnsureDatabaseCreated(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DEMODBContext>();
                context.Database.EnsureCreated();
                RelationalDatabaseCreator databaseCreator =(RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
                databaseCreator.CreateTables();
            }
        }
    }
}
