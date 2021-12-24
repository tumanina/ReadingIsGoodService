using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadingIsGoodService.Data.Entities;

namespace ReadingIsGoodService.Data.Configuration
{

    internal class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Count)
                .IsRequired();

            builder.HasOne(x => x.Order)
                .WithMany(t => t.Items)
                .HasForeignKey("OrderId")
                .IsRequired();

            builder.HasOne(x => x.Product)
                .WithMany(t => t.Items)
                .HasForeignKey("ProductId")
                .IsRequired();
        }
    }
}
