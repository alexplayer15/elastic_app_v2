using elastic_app.api.ServiceConfigurations;
using elastic_app.application;
using MediatR;
using NLog;

var logger = LogManager.Setup().GetCurrentClassLogger();
logger.Debug("Starting Flight Offer API");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Configure();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddMediatR(typeof(RegisterRequestHandler).Assembly);
    builder.Configuration.AddUserSecrets<Program>();

    var app = builder.Build();
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


