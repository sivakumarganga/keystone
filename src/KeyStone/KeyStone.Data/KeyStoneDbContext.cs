using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkCore.AutoHistory.Extensions;

namespace KeyStone.Data
{
    public class KeyStoneDbContext : DbContext
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
            // Enables auto history functionality.
            builder.EnableAutoHistory();
            builder.ApplyConfigurationsFromAssembly(typeof(KeyStoneDbContext).Assembly);
        }
    }
}
