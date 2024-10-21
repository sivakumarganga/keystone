using EntityFrameworkCore.AutoHistory.Extensions;
using KeyStone.Data.Extensions;
using KeyStone.Data.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KeyStone.Data
{
    public class KeyStoneDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public KeyStoneDbContext(DbContextOptions<KeyStoneDbContext> options)
      : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public override int SaveChanges()
        {
            var addedEntities = this.DetectChanges(EntityState.Added);

            this.EnsureAutoHistory();
            var affectedRows = base.SaveChanges();

            this.EnsureAutoHistory(addedEntities);
            affectedRows += base.SaveChanges();

            return affectedRows;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var addedEntities = this.DetectChanges(EntityState.Added);

            this.EnsureAutoHistory();
            var affectedRows = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            this.EnsureAutoHistory(addedEntities);
            affectedRows += await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return affectedRows;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Enables auto history functionality.
            builder.EnableAutoHistory();
            var entitiesAssembly = typeof(IEntity).Assembly;
            builder.RegisterAllEntities<IEntity>(entitiesAssembly);
            builder.ApplyConfigurationsFromAssembly(typeof(KeyStoneDbContext).Assembly);
        }

        /*
         * Migrations Guide
         * 
         * 1. Navigate to src folder
         * 2. Add a migration : dotnet ef migrations add <MigrationName> --project ./KeyStone.Data/KeyStone.Data.csproj --startup-project ./Web/KeyStone.API/KeyStone.API.csproj
         * 3. Update database : dotnet ef database update --project ./KeyStone.Data/KeyStone.Data.csproj --startup-project ./Web/KeyStone.API/KeyStone.API.csproj
         *
         * Note: If you are using a different project structure, please adjust the paths accordingly.
         */
    }
}
