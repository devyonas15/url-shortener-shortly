using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using Persistence.Models;

namespace Persistence.Contexts;

[ExcludeFromCodeCoverage]
public class UrlDbContext : DbContext
{
    public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options)
    {
    }
    
    public DbSet<Url> Urls { get; set; }
    
    public DbSet<UrlMetric> UrlMetrics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UrlConfiguration());
        modelBuilder.ApplyConfiguration(new UrlMetricConfiguration());
        
        // Tell EF for the existence of ApplicationUser from AuthDBContext to map with URL
        modelBuilder.Entity<ApplicationUser>()
            .ToTable("User", x => x.ExcludeFromMigrations());
        
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void SetTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            entry.Entity.DateModified = DateTime.Now;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = DateTime.Now;
            }
        }
    }
}