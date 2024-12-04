namespace NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Models.OpenTelemetry;

public class ExportersOptions
{

    public OtelCollectorOptions OtelCollector { get; set; } = new();

    public AzureMonitorOptions AzureMonitor { get; set; } = new();

}
