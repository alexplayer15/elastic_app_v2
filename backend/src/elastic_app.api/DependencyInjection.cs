using elastic_app.api.Controllers;
using elastic_app.api.Repositories;
using elastic_app.api.Services.UserService;

namespace elastic_app.api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            _ = services.AddTransient<IUserRepository, UserRepository>();
            _ = services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
