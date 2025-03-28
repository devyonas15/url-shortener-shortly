using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class UrlMetricConfiguration : IEntityTypeConfiguration<UrlMetric>
{
    public void Configure(EntityTypeBuilder<UrlMetric> builder)
    {
        builder.ToTable("UrlMetric");
        builder.HasKey(x => x.UrlMetricId);
        
        builder.Property(x => x.UrlMetricId)
            .HasColumnType("int")
            .ValueGeneratedOnAdd();
        builder.Property(x => x.AccessorIp)
            .IsRequired()
            .HasColumnType("nvarchar(45)");
        builder.Property(x => x.UserAgent)
            .IsRequired()
            .HasColumnType("nvarchar(255)");

        builder.HasOne(u => u.Url)
            .WithMany(m => m.Metrics)
            .HasForeignKey(u => u.UrlId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}