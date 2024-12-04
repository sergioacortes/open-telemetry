namespace NetFrameworkOpenTelemetry.Telemetry.Models.Datadog
{
    public class DatadogOptions
    {
        public LogsOptions Logs { get; set; } = new LogsOptions();

        public string ApiKey { get; set; } = string.Empty;
    }

}
