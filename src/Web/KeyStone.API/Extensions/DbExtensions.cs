using KeyStone.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace KeyStone.API.Extensions
{
    public static class DbExtensions
    {
        public static async Task ApplyMigrationsAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetService<DbContext>();

            if (context is null || context.Database is  null)
                throw new Exception("Database Context Not Found");

            if (context.Database.ProviderName is not null && context.Database.ProviderName.Split(".").Last() != nameof(Microsoft.EntityFrameworkCore.InMemory))
            {
                await context.Database.MigrateAsync();
            }
        }
        public static async Task SeedDefaultBusinessEntitiesAsync(this WebApplication app)
        {
            try
            {
                await using var scope = app.Services.CreateAsyncScope();
                var seedService = scope.ServiceProvider.GetRequiredService<IDataSeedService>();
                if (seedService is not null)
                {
                    await seedService.Seed();
                }
            }
            catch (Exception ex) { }
        }
    }
}
