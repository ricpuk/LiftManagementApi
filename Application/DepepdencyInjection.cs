using Application.Interfaces;
using Application.Interfaces.Services;
using Application.LiftService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Application
{
    public static class DepepdencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            
            services.AddSingleton<ILiftService, LiftService.LiftService>();
            services.AddTransient<ILiftScheduler, LiftScheduler.LiftScheduler>();
            return services;
        }
    }
}
