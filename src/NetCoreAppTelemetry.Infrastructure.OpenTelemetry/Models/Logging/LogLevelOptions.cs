using Microsoft.Extensions.Logging;

namespace NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Models.Logging;

public class LogLevelOptions
{
    public LogLevel Default { get; set; } = LogLevel.Information;

}
