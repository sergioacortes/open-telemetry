using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Models;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Extensions;

internal static class TracesServiceCollectionExtensions
{

    internal static IServiceCollection AddTracing(this IServiceCollection services,
        TelemetryOptions telemetryOptions, 
        Action<TracerProviderBuilder> customizeTracing)
    {
        services.AddOpenTelemetry().WithTracing(builder =>
        {
            builder.SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(serviceName: telemetryOptions.ServiceName, serviceVersion: telemetryOptions.ServiceVersion));

            builder
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.RecordException = true;

                    options.Filter = (HttpContext context) =>
                        context.Request.Path.ToUriComponent() != "/" &&
                        !context.Request.Path.ToUriComponent().Contains("index.html", StringComparison.OrdinalIgnoreCase) &&
                        !context.Request.Path.ToUriComponent().Contains("favicon", StringComparison.OrdinalIgnoreCase) &&
                        !context.Request.Path.ToUriComponent().Contains("swagger", StringComparison.OrdinalIgnoreCase) &&
                        !context.Request.Path.ToString().Contains("/status", StringComparison.OrdinalIgnoreCase) &&
                        !(context.Request.Method == HttpMethod.Options.Method && context.Request.Headers.AccessControlRequestMethod.Any());
                })
                .AddHttpClientInstrumentation(options =>
                {
                    options.RecordException = true;
                    options.FilterHttpRequestMessage = (HttpRequestMessage requestMessage) =>
                    {
                        var uriString = requestMessage.RequestUri?.ToString();
                        if (uriString == null)
                            return true;

                        return !uriString.StartsWith("https://northeurope.livediagnostics.monitor.azure.com")
                            && !uriString.StartsWith("https://dc.services.visualstudio.com")
                            && !uriString.StartsWith("https://rt.services.visualstudio.com")
                            && !uriString.StartsWith("https://http-intake.logs.datadoghq.eu");
                    };
                })
#if DEBUG
                .AddConsoleExporter()
#endif
                .AddOtlpExporter(delegate (OtlpExporterOptions opts)
                {
                    if (telemetryOptions.OpenTelemetry.Exporters.OtelCollector.EndpointAddress != null)
                        opts.Endpoint = telemetryOptions.OpenTelemetry.Exporters.OtelCollector.EndpointAddress;
                });

            customizeTracing.Invoke(builder);

        });

        return services;
    }
}