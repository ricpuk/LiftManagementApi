using Application.Interfaces;
using Application.Interfaces.Repositories;
using Infrastructure.FloorSelection;
using Infrastructure.LiftRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<ILiftRepository, InMemoryLiftRepository>();
            services.AddTransient<IFloorSelectionStrategy, FifoFloorSelectionStrategy>();
            return services;
        }
    }
}
