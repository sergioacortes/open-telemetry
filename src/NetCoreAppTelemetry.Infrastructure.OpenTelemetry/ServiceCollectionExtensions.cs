using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Extensions;
using NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Models;
using OpenTelemetry.Trace;

namespace NetCoreAppTelemetry.Infrastructure.OpenTelemetry;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        var options = GetOptions(services, configuration);

        return services
            .AddLogging(options)
            .AddMetrics(options, _ =>
            {
            })
            .AddTracing(options, builder =>
            {
                builder.AddAspNetCoreInstrumentation();
            });
    }
    
    private static TelemetryOptions GetOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TelemetryOptions>()
            .Bind(configuration)
            .Configure(options =>
            {
                var entryAssemblyName = Assembly.GetEntryAssembly()!.GetName();
                if (entryAssemblyName.Name != null) options.ServiceName = entryAssemblyName.Name;
                options.ServiceVersion = entryAssemblyName.Version!.ToString();
            })
            .ValidateDataAnnotations();

        var options = services.BuildServiceProvider().GetRequiredService<IOptions<TelemetryOptions>>().Value;
        
        return options;
    }

}
