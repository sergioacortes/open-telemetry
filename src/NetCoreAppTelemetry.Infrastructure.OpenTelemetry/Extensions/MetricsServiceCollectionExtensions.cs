using Microsoft.Extensions.DependencyInjection;
using NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Extensions;

internal static class MetricsServiceCollectionExtensions
{
    internal static IServiceCollection AddMetrics(this IServiceCollection services,
        TelemetryOptions telemetryOptions, 
        Action<MeterProviderBuilder> customizeMetrics)
    {
        services.AddOpenTelemetry()
            .WithMetrics(opts =>
            {
                opts.SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService(serviceName: telemetryOptions.ServiceName, serviceVersion: telemetryOptions.ServiceVersion));

                opts.AddHttpClientInstrumentation()
                    .AddOtlpExporter(o =>
                    {
                        if (telemetryOptions.OpenTelemetry.Exporters.OtelCollector.EndpointAddress != null)
                            o.Endpoint = telemetryOptions.OpenTelemetry.Exporters.OtelCollector.EndpointAddress;
                    });

                customizeMetrics.Invoke(opts);
            });

        return services;
    }
}