using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Contants;

namespace Persistence.Configurations;

public class CustomerInfoConfiguration : IEntityTypeConfiguration<CustomerInfo>
{
    public void Configure(EntityTypeBuilder<CustomerInfo> builder)
    {
        builder.ToTable(TableNames.CustomerInfo);

        builder.HasKey(x => x.Id);

        //builder.Property(x => x.CustomerId)
        //    .IsRequired();
    }
}