using NetFrameworkOpenTelemetry.Telemetry;
using NetFrameworkOpenTelemetry.Telemetry.Models;
using NetFrameworkOpenTelemetry.Telemetry.Models.Datadog;
using NetFrameworkOpenTelemetry.Telemetry.Models.Logging;
using NetFrameworkOpenTelemetry.Telemetry.Models.OpenTelemetry;
using Owin;
using System;

[assembly: Microsoft.Owin.OwinStartup(typeof(NetFrameworkOpenTelemetry.Host.StartUp))]
namespace NetFrameworkOpenTelemetry.Host
{
    public class StartUp
    {

        public void Configuration(IAppBuilder app)
        {

            var openTelemetryConfiguration = new OpenTelemetryConfiguration() 
            { 
                Environment = "local",
                ServiceName = nameof(StartUp),
                ServiceVersion = "1.0",
                Datadog = new DatadogOptions() 
                { 

                },
                Logging = new LoggingOptions() 
                {
                },
                OpenTelemetry = new OpenTelemetryOptions()
                { 
                    Exporters = new ExportersOptions()
                    { 
                        OtelCollector = new OtelCollectorOptions()
                        { 
                            Enabled = true,
                            EndpointAddress = new Uri("http://localhost:4317")
                        }
                    }
                }
            };

            app.UseOpenTelemetry(openTelemetryConfiguration);

        }

    }
}