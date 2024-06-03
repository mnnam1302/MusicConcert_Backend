using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Contants;

namespace Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(TableNames.Orders);

        builder.HasKey(x => x.Id);

        // Relationships
        builder
            .HasOne(x => x.CustomerInfo)
            .WithMany()
            .HasForeignKey(x => x.CustomerInfoId);

        builder
            .HasMany(x => x.OrderDetails)
            .WithOne()
            .HasForeignKey(x => x.OrderId);
    }
}