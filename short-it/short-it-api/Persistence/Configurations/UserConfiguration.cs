using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Models;

namespace Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("User");
        
        // Set composite key manually for the Identity
        builder.HasKey(x =>  x.Id);

        // Ignore some columns that are not needed from the IdentityUser
        builder.Ignore(x => x.UserName);
        builder.Ignore(x => x.NormalizedUserName);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(254)
            .IsUnicode(false);

        builder.HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("UX_User_Email");
    }
}