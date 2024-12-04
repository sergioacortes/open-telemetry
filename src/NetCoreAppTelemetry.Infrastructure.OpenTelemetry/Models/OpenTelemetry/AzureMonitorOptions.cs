namespace NetCoreAppTelemetry.Infrastructure.OpenTelemetry.Models.OpenTelemetry;

public class AzureMonitorOptions
{
    
    public string InstrumentationKey { get; set; } = string.Empty;
    
    public Uri? IngestionEndpoint { get; set; }
    
    public Uri? LiveEndpoint { get; set; }
    
    public bool Enabled { get; set; }
    
}
