using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Contants;

namespace Persistence.Configurations;

public class CustomerInfoConfiguration
{
    public void Configure(EntityTypeBuilder<CustomerInfo> builder)
    {
        builder.ToTable(TableNames.CustomerInfo);
        builder.HasKey(x => x.Id);
    }
}