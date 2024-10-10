
using EntityFrameworkCore.UnitOfWork.Extensions;
using KeyStone.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace KeyStone.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.development.json",optional:true,reloadOnChange:true);




            builder.Services.AddDbContext<KeyStoneDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("KeyStoneConnection"); // Use your connection string name
                // This example uses SQL Server. Use your provider here
                //options.UseSqlServer(connectionString, sqlServerOptions =>
                //{
                //    var assembly = typeof(KeyStoneDbContext).Assembly;
                //    var assemblyName = assembly.GetName();

                //    sqlServerOptions.MigrationsAssembly(assemblyName.Name);
                //});
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    var assembly = typeof(KeyStoneDbContext).Assembly;
                    var assemblyName = assembly.GetName();
                    npgsqlOptions.MigrationsAssembly(assemblyName.Name);
                });
                //options.UseInMemoryDatabase("MyDatabase");
                options.ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning));
            });

            // Register the DbContext instance
            builder.Services.AddScoped<DbContext, KeyStoneDbContext>();

            // Register the UnitOfWork
            builder.Services.AddUnitOfWork();
            //builder.Services.AddUnitOfWork<KeyStoneDbContext>(); // Multiple databases support

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
