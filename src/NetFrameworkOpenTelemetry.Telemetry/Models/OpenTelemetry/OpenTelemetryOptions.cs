namespace NetFrameworkOpenTelemetry.Telemetry.Models.OpenTelemetry
{
    public class OpenTelemetryOptions
    {
        public ExportersOptions Exporters { get; set; } = new ExportersOptions();

    }
}
