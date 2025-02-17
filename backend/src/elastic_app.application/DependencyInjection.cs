using Microsoft.Extensions.DependencyInjection;
using elastic_app.application.Services.User;
using elastic_app.application.Services.Email;
using elastic_app.application.Services.VerificationToken;
using elastic_app.application.DTOs;
using elastic_app.application.Validations;
using FluentValidation;
using Mapster;
using MapsterMapper;
using System.Reflection;


namespace elastic_app.application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            _ = services.AddTransient<IUserService, UserService>();
            _ = services.AddTransient<IEmailService, EmailService>();
            _ = services.AddTransient<IValidator<RegisterRequest>, RegisterRequestValidation>();
            _ = services.AddTransient<IVerificationTokenService, VerificationTokenService>();
            _ = services.AddMappings();

            return services;
        }

        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;

            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddSingleton<IMapper, Mapper>();

            return services;
        }
    }
}
