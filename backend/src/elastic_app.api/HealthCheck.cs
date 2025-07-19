using Microsoft.Extensions.Diagnostics.HealthChecks;

public class ServiceHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var isHealthy = true;

        if (isHealthy)
        {
            return Task.FromResult(
                HealthCheckResult.Healthy("Elastic App V2 API Is Healthy"));
        }

        return Task.FromResult(
            new HealthCheckResult(
                context.Registration.FailureStatus, "Elastic App V2 API Is Unhealthy"));
    }
}