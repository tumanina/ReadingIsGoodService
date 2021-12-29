using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadingIsGoodService.Data.Repositories;

namespace ReadingIsGoodService.Data.Configuration
{
    public static class DependenciesRegistrator
    {
        public static void ConfigureDataLayer(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration["UseInMemoryDatabase"].Equals("true", System.StringComparison.OrdinalIgnoreCase))
            {
                services.AddDbContext<ReadingIsGoodDbContext>(options => options
                    .UseLazyLoadingProxies(false)
                    .UseInMemoryDatabase("Ordering"));
            }
            else
            {
                services.AddDbContext<ReadingIsGoodDbContext>(options => options
                    .UseLazyLoadingProxies(false)
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            }

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}

