using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class UrlConfiguration : IEntityTypeConfiguration<Url>
{
    public void Configure(EntityTypeBuilder<Url> builder)
    {
        builder.ToTable("Url");
        builder.HasKey(x => x.UrlId);
        
        builder.Property(x => x.UrlId)
            .HasColumnType("int")
            .ValueGeneratedOnAdd();
        builder.Property(x => x.LongUrl)
            .IsRequired()
            .HasColumnType("nvarchar(max)");
        builder.Property(x => x.ShortUrl)
            .IsRequired()
            .HasColumnType("nvarchar(27)");
    }
}