using elastic_app.application;
using elastic_app.infrastructure;
using NLog;

var logger = LogManager.Setup().GetCurrentClassLogger();
logger.Debug("Starting Flight Offer API");

try
{
    var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true)
    .AddEnvironmentVariables()
    .Build();

    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(configuration);
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

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
