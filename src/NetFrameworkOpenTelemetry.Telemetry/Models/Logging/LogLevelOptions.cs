using Microsoft.Extensions.Logging;

namespace NetFrameworkOpenTelemetry.Telemetry.Models.Logging
{
    public class LogLevelOptions
    {
        public LogLevel Default { get; set; } = LogLevel.Information;
    }
}
