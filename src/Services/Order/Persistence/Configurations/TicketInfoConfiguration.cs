using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Contants;

namespace Persistence.Configurations;

public class TicketInfoConfiguration : IEntityTypeConfiguration<TicketInfo>
{
    public void Configure(EntityTypeBuilder<TicketInfo> builder)
    {
        builder.ToTable(TableNames.TicketInfo);

        builder.HasKey(x => x.Id);

        //builder.Property(x => x.TicketId)
        //    .IsRequired();
    }
}