using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

internal class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable(TableNames.Events);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(255);

        //builder.Property(x => x.StartDate)
        //    .IsRequired();

        //builder.Property(x => x.StartTime)
        //    .IsRequired();

        //builder.Property(x => x.EndTime)
        //    .IsRequired();

        builder.Property(x => x.StartedOnUtc)
            .IsRequired();

        builder.Property(x => x.EndedOnUtc)
            .IsRequired();

        builder.Property(x => x.Capacity)
            .IsRequired();

        builder.Property(x => x.OrganizationInfoId)
            .IsRequired();

        builder.Property(x => x.EventType)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.MeetUrl)
            .HasMaxLength(255);

        builder.Property(x => x.Address)
            .HasMaxLength(100);

        builder.Property(x => x.District)
            .HasMaxLength(20);

        builder.Property(x => x.City)
            .HasMaxLength(30);

        builder.Property(x => x.Country)
            .HasMaxLength(30);

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId);

        //builder.HasOne(x => x.OrganizationInfo)
        //    .WithMany()
        //    .HasForeignKey(x => x.OrganizationInfoId);
    }
}