using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Contants;

namespace Persistence.Configurations;

public class OrderInfoConfiguration : IEntityTypeConfiguration<OrderInfo>
{
    public void Configure(EntityTypeBuilder<OrderInfo> builder)
    {
        builder.ToTable(TableNames.OrderInfo);

        builder.HasKey(x => x.Id);
    }
}