using Application.Interfaces;
using Application.Interfaces.Repositories;
using Infrastructure.FloorSelection;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<ILiftRepository, InMemoryLiftRepository>();
            services.AddSingleton<ILiftLogRepository, InMemoryLiftLogRepository>();
            services.AddSingleton<ILiftOperationRepository, LiftOperationRepository>();
            services.AddTransient<IFloorSelectionStrategy, FifoFloorSelectionStrategy>();
            return services;
        }
    }
}
