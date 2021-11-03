using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.MongoDB;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(EFRepository<>));
            services.AddTransient(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            return services;
        }
    }
}