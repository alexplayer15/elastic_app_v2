using Microsoft.Extensions.DependencyInjection;
using elastic_app.application.Services.User;
using elastic_app.application.DTOs;
using elastic_app.application.Validations;
using FluentValidation;


namespace elastic_app.application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            _ = services.AddTransient<IUserService, UserService>();
            _ = services.AddTransient<IValidator<RegisterRequest>, RegisterRequestValidation>();

            return services;
        }
    }
}
