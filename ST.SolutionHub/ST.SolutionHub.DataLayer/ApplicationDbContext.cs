using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ST.SolutionHub.DataLayer
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(builder);
        }
        public override int SaveChanges()
        {
            SetAuditProperties();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetAuditProperties();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public async override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetAuditProperties();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditProperties();
            return await base.SaveChangesAsync(cancellationToken);
        }
        public void SetAuditProperties()
        {
            var now = DateTime.UtcNow;
            string userId = null;
            ChangeTracker.DetectChanges();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                if (entry.State == EntityState.Added && entry.Metadata.FindProperty("CreatedDate") != null)
                {
                    entry.Property("CreatedDate").CurrentValue = now;
                    entry.Property("UpdatedDate").CurrentValue = now;
                    entry.Property("CreatedBy").CurrentValue = userId;
                    entry.Property("UpdatedBy").CurrentValue = userId;
                }

                if (entry.State == EntityState.Modified && entry.Metadata.FindProperty("UpdatedDate") != null)
                {
                    entry.Property("UpdatedDate").CurrentValue = now;
                    entry.Property("UpdatedBy").CurrentValue = userId;
                }
            }
        }
    }
}
