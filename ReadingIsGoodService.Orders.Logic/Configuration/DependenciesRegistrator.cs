using Microsoft.Extensions.DependencyInjection;
using ReadingIsGoodService.Orders.Logic.Interfaces;

namespace ReadingIsGoodService.Orders.Logic.Configuration
{
    public static class DependenciesRegistrator
    {
        public static void ConfigureLogic(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
        }
    }
}

