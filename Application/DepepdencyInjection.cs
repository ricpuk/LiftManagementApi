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
            
            services.AddTransient<ILiftService, LiftService.LiftService>();
            return services;
        }
    }
}
