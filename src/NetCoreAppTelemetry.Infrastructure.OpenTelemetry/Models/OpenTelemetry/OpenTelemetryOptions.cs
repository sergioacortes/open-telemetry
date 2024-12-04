namespace NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Models.OpenTelemetry;

public class OpenTelemetryOptions
{
    public ExportersOptions Exporters { get; set; } = new();

}
