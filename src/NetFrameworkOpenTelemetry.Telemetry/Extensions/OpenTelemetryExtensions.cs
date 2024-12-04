using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using NetFrameworkOpenTelemetry.Telemetry.Models;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Owin;

namespace NetFrameworkOpenTelemetry.Telemetry
{
    public static class OpenTelemetryExtensions
    {
        public static IAppBuilder UseOpenTelemetry(this IAppBuilder app, OpenTelemetryConfiguration openTelemetryConfiguration)
        {
            
            var assemblyName = nameof(OpenTelemetryExtensions);
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            _ = Sdk.CreateTracerProviderBuilder()
               .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                                .AddService(serviceName: assemblyName, serviceVersion: version)
                                .AddAttributes(new[]
                                {
                                    new KeyValuePair<string, object>("env", openTelemetryConfiguration.Environment)
                                }))
               .AddHttpClientInstrumentation((options) =>
               {
                   options.RecordException = true;
                   options.FilterHttpWebRequest = (message) => FilterHttpWebRequest(message.RequestUri?.ToString() ?? string.Empty);
               })
               .AddAspNetInstrumentation((options) =>
               {
                   options.RecordException = true;
                   options.Filter = (httpContext) => !IsIgnoredPath(httpContext.Request.Path) && !IsOptionsMethod(HttpContext.Current.Request.HttpMethod);
               })
               .AddConsoleExporter()
               .AddOtlpExporter(opt =>
               {
                   opt.Endpoint = openTelemetryConfiguration.OpenTelemetry.Exporters.OtelCollector.EndpointAddress;
               })
               .AddSource("Wke.*", "a3ERP.*")
               .AddSource("NServiceBus.*")
               .SetSampler<AlwaysOnSampler>()
               .Build();

            Sdk.CreateMeterProviderBuilder()
               .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                                .AddService(serviceName: assemblyName, serviceVersion: version)
                                .AddAttributes(new[]
                                {
                                    new KeyValuePair<string, object>("env", openTelemetryConfiguration.Environment)
                                }))
               .AddHttpClientInstrumentation()
               .AddAspNetInstrumentation()
               .AddMeter("NServiceBus.*")
               .AddMeter("NetFrameworkOpenTelemetry.*")
               .AddOtlpExporter(opt =>
               {
                   opt.Endpoint = openTelemetryConfiguration.OpenTelemetry.Exporters.OtelCollector.EndpointAddress;
               })
               .Build();

            return app;
        }

        private static bool IsIgnoredPath(string path)
        {
            var ignoredPaths = new[] { "index.html", "favicon", "swagger", "/health", "/healthz", "/status" };
            return ignoredPaths.AsEnumerable().Any(path.Contains);
        }

        private static bool FilterHttpWebRequest(string requestUri)
        {
            return !requestUri.Contains("livediagnostics.monitor.azure.com") &&
                   !requestUri.StartsWith("https://dc.services.visualstudio.com") &&
                   !requestUri.StartsWith("https://rt.services.visualstudio.com") &&
                   !requestUri.Contains("applicationinsights.azure.com") &&
                   !requestUri.Contains("https://http-intake.logs.datadoghq");
        }

        private static bool IsOptionsMethod(string method)
        {
            return method == "OPTIONS";
        }
    }
}
