using NetFrameworkOpenTelemetry.Telemetry.Models.Datadog;
using NetFrameworkOpenTelemetry.Telemetry.Models.Logging;
using NetFrameworkOpenTelemetry.Telemetry.Models.OpenTelemetry;

namespace NetFrameworkOpenTelemetry.Telemetry.Models
{
    public class OpenTelemetryConfiguration
    {

        public string Environment { get; set; } = string.Empty;

        public LoggingOptions Logging { get; set; } = new LoggingOptions();

        public DatadogOptions Datadog { get; set; } = new DatadogOptions();

        public OpenTelemetryOptions OpenTelemetry { get; set; } = new OpenTelemetryOptions();

        public string ServiceName { get; set; } = string.Empty;

        public string ServiceVersion { get; set; } = string.Empty;

    }
}
