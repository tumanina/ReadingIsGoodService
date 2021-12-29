using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadingIsGoodService.Data.Entities;

namespace ReadingIsGoodService.Data.Configuration
{
    internal class ActivityLogEntityConfiguration : IEntityTypeConfiguration<ActivityLogEntity>
    {
        public void Configure(EntityTypeBuilder<ActivityLogEntity> builder)
        {
            builder.ToTable("ActivityLog");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EntityId)
                .IsRequired();
        }
    }
}
