using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

internal class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable(TableNames.Organizations);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Industry)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(250);

        builder.Property(x => x.Phone)
            .HasMaxLength(20);

        builder.Property(x => x.HomePage)
            .HasMaxLength(150);

        builder.Property(x => x.LogoUrl)
            .HasMaxLength(150);

        builder.Property(x => x.Street)
            .HasMaxLength(50);

        builder.Property(x => x.City)
            .HasMaxLength(30);

        builder.Property(x => x.Country)
            .HasMaxLength(30);

        builder.Property(x => x.State)
            .HasMaxLength(50);

        builder.Property(x => x.ZipCode)
            .HasMaxLength(20);

        //builder.OwnsOne(x => x.Address, address =>
        //{
        //    address.Property(x => x.Street)
        //        .HasMaxLength(50)
        //        .HasColumnName("Street");

        //    address.Property(x => x.City)
        //        .HasMaxLength(30)
        //        .HasColumnName("City");

        //    address.Property(x => x.State)
        //        .HasMaxLength(50)
        //        .HasColumnName("State");

        //    address.Property(x => x.Country)
        //        .HasMaxLength(30)
        //        .HasColumnName("Country");

        //    address.Property(x => x.ZipCode)
        //        .HasMaxLength(20)
        //        .HasColumnName("ZipCode");
        //});
    }
}