using Microsoft.Extensions.DependencyInjection;
using elastic_app.application.Services.UserService;


namespace elastic_app.application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            _ = services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
