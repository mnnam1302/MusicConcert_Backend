using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

internal class AppCustomerConfiguration : IEntityTypeConfiguration<AppCustomer>
{
    public void Configure(EntityTypeBuilder<AppCustomer> builder)
    {
        builder.ToTable(TableNames.AppCustomers);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName).HasMaxLength(20);

        builder.Property(x => x.LastName).HasMaxLength(20);

        builder.Property(x => x.FullName).HasMaxLength(40);

        builder.Property(x => x.Email)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(50);

        builder.Property(x => x.PasswordHash).IsRequired();
        builder.Property(x => x.PasswordSalt).IsRequired();

        builder.Property(x => x.PhoneNumber).HasMaxLength(20);

        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(x => x.Street)
                .HasMaxLength(30)
                .HasColumnName("Street");

            address.Property(x => x.City)
                .HasMaxLength(30)
                .HasColumnName("City");

            address.Property(x => x.State)
                .HasMaxLength(30)
                .HasColumnName("State");

            address.Property(x => x.Country)
                .HasMaxLength(30)
                .HasColumnName("Country");

            address.Property(x => x.ZipCode)
                .HasMaxLength(20)
                .HasColumnName("ZipCode");
        });

        // Each User can have many UserClaims
        builder.HasMany(e => e.Claims)
            .WithOne()
            .HasForeignKey(userClaim => userClaim.UserId)
            .IsRequired();

        // Each User can have many UserLogins
        builder.HasMany(e => e.Logins)
            .WithOne()
            .HasForeignKey(userLogin => userLogin.UserId)
            .IsRequired();

        // Each User can have many UserTokens
        builder.HasMany(e => e.Tokens)
            .WithOne()
            .HasForeignKey(userToken => userToken.UserId)
            .IsRequired();

        builder.HasOne(x => x.AppRole)
            .WithMany()
            .HasForeignKey(x => x.RoleId);
    }
}