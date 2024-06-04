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

        builder.Property(x => x.CustomerId)
            .IsRequired();

        builder.Property(x => x.FullName)
            .IsRequired();

        builder.Property(x => x.Email)
            .IsRequired();

        builder.Property(x => x.PhoneNumer)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);
    }
}