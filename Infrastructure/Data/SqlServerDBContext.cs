using Domain.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Infrastructure.Data
{
    public class SqlServerDBContext : DbContext
    {
        public SqlServerDBContext(DbContextOptions<SqlServerDBContext> options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableModel && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((AuditableModel)entityEntry.Entity).DateModified = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((AuditableModel)entityEntry.Entity).DateCreated = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(p => p.Email).IsUnique();
            modelBuilder.Entity<Customer>();
            modelBuilder.Entity<CommonCode>().HasIndex(p => new { p.Name, p.Code }).IsUnique();
            modelBuilder.Entity<Location>().HasIndex(p => new { p.Name, p.Country }).IsUnique();
            modelBuilder.Entity<Shipment>().HasIndex(p => p.Code).IsUnique();
            modelBuilder.Entity<FixOrder>().HasIndex(p => p.Number).IsUnique();
            modelBuilder.Entity<AppConfig>().HasIndex(p => p.Key).IsUnique();
            modelBuilder.Entity<Element>();
            modelBuilder.Entity<Fix>();
        }

        //private void SetAuditableModelModificationInfo(AuditableModel model)
        //{
        //    var currentUser = User.
        //    var currentUserId = currentUser != null ? currentUser.Id : (int?)null;
        //    if (model.IsTransient())
        //    {
        //        model.CreatedBy = currentUserId;
        //        model.CreatedOnUtc = this._clock.UtcNow;
        //    }
        //    else
        //    {
        //        model.ModifiedBy = currentUserId;
        //        model.ModifiedOnUtc = this._clock.UtcNow;
        //    }
        //}
    }
}