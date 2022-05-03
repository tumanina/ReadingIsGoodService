using Microsoft.Extensions.DependencyInjection;
using ReadingIsGoodService.Customers.Logic.Interfaces;

namespace ReadingIsGoodService.Customers.Logic.Configuration
{
    public static class DependenciesRegistrator
    {
        public static void ConfigureLogic(this IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>();
        }
    }
}

