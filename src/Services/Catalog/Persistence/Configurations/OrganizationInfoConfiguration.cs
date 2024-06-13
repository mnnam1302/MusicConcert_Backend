using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

public class OrganizationInfoConfiguration : IEntityTypeConfiguration<OrganizationInfo>
{
    public void Configure(EntityTypeBuilder<OrganizationInfo> builder)
    {
        builder.ToTable(TableNames.OrganizationInfo);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
    }
}