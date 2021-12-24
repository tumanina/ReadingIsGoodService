using Microsoft.EntityFrameworkCore;
using ReadingIsGoodService.Data.Configuration;
using ReadingIsGoodService.Data.Entities;

namespace ReadingIsGoodService.Data
{
    class ReadingIsGoodDbContext : DbContext
    {
        public ReadingIsGoodDbContext(DbContextOptions<ReadingIsGoodDbContext> dbOptions) : base(dbOptions)
        { }

        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<ProductEntity> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CustomerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemEntityConfiguration());
        }
    }
}
