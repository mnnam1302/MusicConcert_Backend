using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable(TableNames.Tickets);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.UnitPrice)
            .IsRequired();

        builder.Property(x => x.UnitInStock)
            .IsRequired();

        builder.HasOne(x => x.Event)
            .WithMany()
            .HasForeignKey(x => x.EventId);
    }
}