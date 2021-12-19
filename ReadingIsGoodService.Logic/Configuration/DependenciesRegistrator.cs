using Microsoft.Extensions.DependencyInjection;
using ReadingIsGoodService.Logic.Interfaces;

namespace ReadingIsGoodService.Logic.Configuration
{
    public static class DependenciesRegistrator
    {
        public static void ConfigureLogic(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICustomerService, CustomerService>();
        }
    }
}

