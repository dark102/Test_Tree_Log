using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Test_Tree_Log.Models;

namespace Test_Tree_Log.Service

{
    public class ApplicationDbContext : DbContext
    {
        private string conStr;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Tree> tree { get; set; }
        public DbSet<Child> child { get; set; }
        public DbSet<ExceptionJournal> exceptionJournal { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tree>().HasKey(k => k.id);
            modelBuilder.Entity<Tree>(entity => {entity.HasIndex(e => e.name).IsUnique();});

            modelBuilder.Entity<Child>().HasKey(k => k.id);
            modelBuilder.Entity<Child>(entity => { entity.HasIndex(e => e.name).IsUnique(); });

            modelBuilder.Entity<ExceptionJournal>().HasKey(k => k.id);
        }

        public override int SaveChanges()
        {
            AddAuditInfo();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddAuditInfo();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            AddAuditInfo();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        private void AddAuditInfo()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity &&
                           (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                var entity = (IAuditableEntity)entry.Entity;
                var now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.createdDate = now;
                }
                else
                {
                    base.Entry(entity).Property(x => x.createdDate).IsModified = false;
                }
                entity.updatedDate = now;
            }
        }
    }
}
