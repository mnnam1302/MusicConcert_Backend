using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

internal class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.ToTable(TableNames.AppRoles);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description).HasMaxLength(250).IsRequired(true);
        builder.Property(x => x.RoleCode).HasMaxLength(50).IsRequired(true);

        // Each AppRole can have many entries in the UserRole join table
        builder.HasMany(role => role.UserRoles)
            .WithOne()
            .HasForeignKey(userRole => userRole.RoleId)
            .IsRequired();

        // Each AppRole can have many AppCustomers
        //builder.HasMany(role => role.AppCustomers)
        //    .WithOne()
        //    .HasForeignKey(customer => customer.RoleId);
    }
}