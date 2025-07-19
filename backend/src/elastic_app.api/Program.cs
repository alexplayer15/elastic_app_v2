using elastic_app.api.ServiceConfigurations;
using elastic_app.application.Handlers;
using elastic_app.infrastructure.DynamoDB;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NLog;

var logger = LogManager.Setup().GetCurrentClassLogger();
logger.Debug("Starting Flight Offer API");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Configure();
    builder.Services.AddHealthChecks()
    .AddCheck<ServiceHealthCheck>("service")
    .AddCheck<DynamoDbHealthCheck>("dynamodb");
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddMediatR(typeof(RegisterRequestHandler).Assembly);
    builder.Services.AddMediatR(typeof(EmailVerificationHandler).Assembly);
    builder.Configuration.AddUserSecrets<Program>();
    builder.Services.AddLogging();

    var app = builder.Build();
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        Predicate = check => check.Name == "service"
    });

    app.MapHealthChecks("/health/table", new HealthCheckOptions
    {
        Predicate = check => check.Name == "dynamodb"
    });
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}

public partial class Program { }


