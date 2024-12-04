using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Models;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Serilog;
using Serilog.Enrichers.Datadog.OpenTelemetry;
using Serilog.Events;
using Serilog.Sinks.Datadog.Logs;

namespace NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Extensions;

internal static class LoggingServiceCollectionExtensions
{

    internal static IServiceCollection AddLogging(this IServiceCollection services, TelemetryOptions options)
    {
        return services
            .AddLogging(builder =>
            {

                builder.ClearProviders();

                AddLoggingToDatadog(builder, options);
                AddLoggingToOpenTelemetry(builder, options);

            });
    }

    private static void AddLoggingToDatadog(ILoggingBuilder builder, TelemetryOptions options)
    {
        if (!options.Datadog.Logs.Enabled)
            return;

        builder.AddSerilog(new LoggerConfiguration()
            .MinimumLevel.Is((LogEventLevel)options.Logging.LogLevel.Default)
            .Enrich.WithProperty("env", options.Environment)
            .Enrich.FromLogContext()
            .Enrich.WithDatadogTraceId()
            .Enrich.WithDatadogSpanId()
            .WriteTo.Console()
            .WriteTo.DatadogLogs(options.Datadog.ApiKey,
                service: options.ServiceName,
                configuration: new DatadogConfiguration(url: options.Datadog.Logs.Endpoint!.ToString()))
            .CreateLogger());
    }

    private static void AddLoggingToOpenTelemetry(ILoggingBuilder builder, TelemetryOptions telemetryOptions)
    {
        builder.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(serviceName: telemetryOptions.ServiceName, serviceVersion: telemetryOptions.ServiceVersion));

            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.AddOtlpExporter(opts =>
            {
                if (telemetryOptions.OpenTelemetry.Exporters.OtelCollector.EndpointAddress != null)
                    opts.Endpoint = telemetryOptions.OpenTelemetry.Exporters.OtelCollector.EndpointAddress;
            });
        });
    }
}
