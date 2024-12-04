using System;

namespace NetFrameworkOpenTelemetry.Telemetry.Models.Datadog
{
    public class LogsOptions
    {
        public bool Enabled { get; set; }

        public Uri Endpoint { get; set; }

    }
}
