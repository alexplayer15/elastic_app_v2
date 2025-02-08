using elastic_app.application;
using elastic_app.infrastructure;

namespace elastic_app.api.ServiceConfigurations
{
    public static class ConfigureComponents
    {
        public static void Configure(this WebApplicationBuilder builder)
        {
            var environmentName =
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables()
                .AddSystemsManager(source =>
                {
                    source.Path = "/ecs/elastic-app";
                    source.ReloadAfter = TimeSpan.FromSeconds(30);
                    source.Optional = !IsProduction(environmentName);
                })
                .Build();

            _ = builder.Services.AddApplication();
            _ = builder.Services.AddInfrastructure(configuration);
        }
        private static bool IsProduction(string environmentName)
        {
            if (string.IsNullOrWhiteSpace(environmentName))
                return false;

            if (environmentName.Equals("Development", StringComparison.CurrentCultureIgnoreCase) ||
                    environmentName.Equals("Integration", StringComparison.CurrentCultureIgnoreCase) ||
                    environmentName.Equals("local", StringComparison.CurrentCultureIgnoreCase))
                return false;

            return true;
        }
    }
}
