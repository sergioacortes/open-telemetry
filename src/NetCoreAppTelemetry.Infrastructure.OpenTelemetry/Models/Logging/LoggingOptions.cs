namespace NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Models.Logging;

public class LoggingOptions
{
    public LogLevelOptions LogLevel { get; set; } = new();

}
