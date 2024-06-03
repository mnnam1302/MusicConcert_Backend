using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Contants;

namespace Persistence.Configurations;

public class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetails>
{
    public void Configure(EntityTypeBuilder<OrderDetails> builder)
    {
        builder.ToTable(TableNames.OrderDetails);

        builder.HasKey(x => x.Id);

        // Relationships
        builder
            .HasOne(x => x.TicketInfo)
            .WithMany()
            .HasForeignKey(x => x.TicketInfoId);
    }
}