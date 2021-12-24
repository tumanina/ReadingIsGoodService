using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadingIsGoodService.Data.Entities;

namespace ReadingIsGoodService.Data.Configuration
{
    internal class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Customer)
                .WithMany(t => t.Orders)
                .HasForeignKey("CustomerId")
                .IsRequired();
        }
    }
}
