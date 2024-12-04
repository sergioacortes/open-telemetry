using System;

namespace NetFrameworkOpenTelemetry.Telemetry.Models.OpenTelemetry
{
    public class OtelCollectorOptions
    {

        public Uri EndpointAddress { get; set; }

        public bool Enabled { get; set; }

    }
}
