using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using Docker.DotNet;
using Docker.DotNet.Models;
using Testcontainers.LocalStack;
using Reqnroll;

namespace elastic_app.integration.tests.Hooks
{
    public class IntegrationTestHooks
    {
        public async Task<bool> LocalStackContainerExists()
        {
            return await CheckContainerExists(HookConstants.LocalStackTestContainerName);
        }

        public static async Task<bool> CheckContainerExists(string containerName)
        {
            using var dockerClient = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock"))
                .CreateClient();

            var containers = await dockerClient.Containers.ListContainersAsync(new ContainersListParameters
            {
                All = true
            });

            return containers.Any(c => c.Names.Contains($"/{containerName}"));
        }
        public async Task SetUpLocalStack()
        {
            var localStackContainer = new LocalStackBuilder()
                .WithName(HookConstants.LocalStackTestContainerName)
                .WithResourceMapping("../../../../../../.env", "/run/secrets/email_credentials", UnixFileModes.UserRead)
                .WithResourceMapping("../../../../../../localStackScripts", "etc/localstack/init/ready.d", UnixFileModes.UserExecute)
                .WithPortBinding(4566, true)
                .WithEnvironment("DYNAMODB_SHARE_DB", "1")
                .WithEnvironment("AWS_ACCESS_KEY_ID", "DUMMYIDEXAMPLE")
                .WithEnvironment("AWS_SECRET_ACCESS_KEY", "DUMMYEXAMPLEKEY")
                .WithBindMount("/var/run/docker.sock", "/var/run/docker.sock")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("Token Data table populated"))
                .Build();
            await localStackContainer.StartAsync();

            Environment.SetEnvironmentVariable("AWS__ServiceUrl", localStackContainer.GetConnectionString());
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", "DUMMYIDEXAMPLE");
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", "DUMMYEXAMPLEKEY");
        }
    }
}
