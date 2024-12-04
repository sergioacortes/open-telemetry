namespace NetFrameworkOpenTelemetry.Telemetry.Models.OpenTelemetry
{
    public class ExportersOptions
    {

        public OtelCollectorOptions OtelCollector { get; set; } = new OtelCollectorOptions();
        public AzureMonitorOptions AzureMonitor { get; set; } = new AzureMonitorOptions();

    }
}
