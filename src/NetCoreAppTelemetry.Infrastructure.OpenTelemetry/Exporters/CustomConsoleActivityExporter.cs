using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using OpenTelemetry;
using OpenTelemetry.Exporter;

namespace NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Exporters;

[ExcludeFromCodeCoverage]
public class CustomConsoleActivityExporter : ConsoleExporter<Activity>
{
    public const string NServiceBusSourceName = "NServiceBus.Extensions.Diagnostics";

    public CustomConsoleActivityExporter()
        : base(new ConsoleExporterOptions())
    {
    }

    public override ExportResult Export(in Batch<Activity> batch)
    {
        foreach (var activity in batch)
        {
            var description = GetDescription(activity);
            var error = activity.Status == ActivityStatusCode.Error ? $". ERR: {activity.StatusDescription}" : "";

            WriteLine($"[{activity.StartTimeUtc:HH:mm:ss} TRC] {description}{error}");
        }

        return ExportResult.Success;
    }

    private static ReadOnlySpan<char> GetDescription(Activity activity)
    {
        if (activity.Source.Name == NServiceBusSourceName)
        {
            if (activity.Kind == ActivityKind.Consumer)
            {
                var originatingEndpoint = GetTag(activity, "messaging.nservicebus.originatingendpoint");
                var messagetype = GetFirstElement(GetTag(activity, "messaging.nservicebus.enclosedmessagetypes"));
                var operation = GetTag(activity, "messaging.operation");
                return $"NServiceBus {activity.Kind}: {messagetype.ToString()} {operation}, from {originatingEndpoint}";
            }

            return $"NServiceBus {activity.Kind}: {activity.DisplayName}";
        }
        
        return $"{activity.Source.Name} {activity.Kind}: {activity.DisplayName}";
    }

    private static ReadOnlySpan<char> GetFirstElement(string commaSeparatedList)
    {
        int firstComma = commaSeparatedList.IndexOf(',');
        if (firstComma == -1)
            return commaSeparatedList.AsSpan();
        return commaSeparatedList.AsSpan(0, firstComma);
    }

    private static string GetTag(Activity activity, string tagName)
    {
        foreach (var tag in activity.TagObjects)
        {
            if (tag.Key == tagName)
                return tag.Value?.ToString() ?? string.Empty;
        }

        return "";
    }
}
