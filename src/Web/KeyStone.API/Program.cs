
using EntityFrameworkCore.UnitOfWork.Extensions;
using KeyStone.API.Extensions;
using KeyStone.API.ServiceConfiguration;
using KeyStone.Core.Contracts;
using KeyStone.Core.Services;
using KeyStone.Data;
using KeyStone.Data.Extensions;
using KeyStone.Domain;
using KeyStone.Domain.Services;
using KeyStone.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using KeyStone.Identity.Dtos;
using KeyStone.Identity.ServiceConfiguration;
using Microsoft.Extensions.Configuration;

namespace KeyStone.API
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                ApplicationName = "KeyStone.API",
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "allowBlazorLocal",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:8086")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                    });
            });
            builder.Configuration.AddJsonFile("appsettings.json")
                                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
                                .AddEnvironmentVariables()
                                .AddUserSecrets(Assembly.GetEntryAssembly()!);

            builder.Services.Configure<IdentitySettings>(builder.Configuration.GetSection(nameof(IdentitySettings)));



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
                options.UseNpgsql(connectionString, npgSqlOptions =>
                {
                    var assembly = typeof(KeyStoneDbContext).Assembly;
                    var assemblyName = assembly.GetName();
                    npgSqlOptions.MigrationsAssembly(assemblyName.Name);
                });
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                //options.UseInMemoryDatabase("MyDatabase");
                options.ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning));
            });

            // Register the DbContext instance
            builder.Services.AddScoped<DbContext, KeyStoneDbContext>();

            // Register the UnitOfWork
            builder.Services.AddUnitOfWork();
            //builder.Services.AddUnitOfWork<KeyStoneDbContext>(); // Multiple databases support
            builder.Services.AddCustomDataServices();

            // Add services to the container.
            builder.Services.AddScoped<ISampleContract, SampleEntityService>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddScoped<IDataSeedService, BasicDataSeeder>();
            builder.Services.AddSwagger();
            builder.Services.AddHttpContextAccessor();
            builder.Services.ConfigureRequestContext();
            builder.Services.ConfigureRequestValidators();
            var identitySettings = builder.Configuration.GetSection(nameof(IdentitySettings)).Get<IdentitySettings>();
            builder.Services.RegisterIdentityServices(identitySettings: identitySettings);
            builder.Services.AddWebFrameworkServices();

            builder.Services.AddApiVersioning();

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddAutoMapper(typeof(DomainMapperProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(IdentityMapperProfile).Assembly);

            builder.Services.AddDistributedMemoryCache();
            var app = builder.Build();

            await app.ApplyMigrationsAsync();
            await app.SeedDefaultUsers();
            await app.SeedDefaultBusinessEntitiesAsync();
            app.UseSwaggerAndUI();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            
            app.UseCors("allowBlazorLocal");
            
            app.UseAuthorization();

            app.ConfigureCustomMiddlewares();

            app.MapControllers();

            app.Run();
        }
    }
}
