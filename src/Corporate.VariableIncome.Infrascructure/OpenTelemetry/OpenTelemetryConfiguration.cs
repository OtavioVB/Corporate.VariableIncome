using OpenTelemetry;
using OpenTelemetry.Exporter;

namespace Corporate.VariableIncome.Infrascructure.OpenTelemetry;

public sealed class OpenTelemetryConfiguration
{
    public string Endpoint { get; set; } = string.Empty;
    public ExportProcessorType ExportProcessorType { get; set; }
    public OtlpExportProtocol Protocol { get; set; }
}
