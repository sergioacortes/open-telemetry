namespace NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Models.Datadog;

public class LogsOptions
{
    public bool Enabled { get; set; }

    public Uri? Endpoint { get; set; }

}
