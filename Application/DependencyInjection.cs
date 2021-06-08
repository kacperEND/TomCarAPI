using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ICommonCodeService, CommonCodeService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IFixService, FixService>();
            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<IAppConfigService, AppConfigService>();
            return services;
        }
    }
}