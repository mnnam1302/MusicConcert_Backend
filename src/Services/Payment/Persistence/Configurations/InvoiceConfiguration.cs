using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Contants;

namespace Persistence.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable(TableNames.Invoices);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SubTotal)
            .IsRequired();

        builder.Property(x => x.Discount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.Tax)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.TotalPrice)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(20);

        // Relationships
        builder.HasOne(x => x.CustomerInfo)
            .WithMany()
            .HasForeignKey(x => x.CustomerInfoId);

        builder.HasOne(x => x.OrderInfo)
            .WithMany()
            .HasForeignKey(x => x.OrderInfoId);
    }
}