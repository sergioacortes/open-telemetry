using System.ComponentModel.DataAnnotations;
using NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Models.Datadog;
using NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Models.Logging;
using NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Models.OpenTelemetry;

namespace NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Models;

public class TelemetryOptions
{
    [Required] 
    public string Environment { get; set; } = string.Empty;

    public LoggingOptions Logging { get; set; } = new();

    public DatadogOptions Datadog { get; set; } = new();

    public OpenTelemetryOptions OpenTelemetry { get; set; } = new();

    public string ServiceName { get; set; }  = string.Empty;
    
    public string ServiceVersion { get; set; }  = string.Empty;

}
