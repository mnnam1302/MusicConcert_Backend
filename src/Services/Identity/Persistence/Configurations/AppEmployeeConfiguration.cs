using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

internal class AppEmployeeConfiguration : IEntityTypeConfiguration<AppEmployee>
{
    public void Configure(EntityTypeBuilder<AppEmployee> builder)
    {
        builder.ToTable(TableNames.AppEmployees);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName).HasMaxLength(20);

        builder.Property(x => x.LastName).HasMaxLength(20);

        builder.Property(x => x.FullName).HasMaxLength(40);

        builder.Property(x => x.Email)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.EmailConfirmed)
            .HasMaxLength(50);

        builder.Property(x => x.PasswordHash).IsRequired();
        builder.Property(x => x.PasswordSalt).IsRequired();

        builder.Property(x => x.PhoneNumber).HasMaxLength(20);

        builder.Property(x => x.IsDirector).HasDefaultValue(false);
        builder.Property(x => x.IsHeadOfDepartment).HasDefaultValue(false);
        builder.Property(x => x.ManagerId).HasDefaultValue(null);
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);

        // Each User can have many UserClaims
        builder.HasMany(e => e.Claims)
            .WithOne()
            .HasForeignKey(uc => uc.UserId)
            .IsRequired();

        // Each User can have many UserLogins
        builder.HasMany(e => e.Logins)
            .WithOne()
            .HasForeignKey(ul => ul.UserId)
            .IsRequired();

        // Each User can have many UserTokens
        builder.HasMany(e => e.Tokens)
            .WithOne()
            .HasForeignKey(ut => ut.UserId)
            .IsRequired();

        // Each User can have many Roles
        builder.HasMany(e => e.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
    }
}