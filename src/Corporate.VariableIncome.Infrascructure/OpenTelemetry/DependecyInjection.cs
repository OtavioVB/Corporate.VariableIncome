using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace Corporate.VariableIncome.Infrascructure.OpenTelemetry;

public static class DependecyInjection
{
    public static ILoggingBuilder AddOpenTelemetryConfiguration(
        this ILoggingBuilder logging,
        IConfiguration configuration)
    {
        var openTelemetryConfiguration = configuration.GetRequiredSection(nameof(OpenTelemetryConfiguration)).Get<OpenTelemetryConfiguration>()!;

        logging.AddOpenTelemetry(options =>
        {
            options.IncludeScopes = true;
            options.IncludeFormattedMessage = true;
            options.ParseStateValues = true;
            options.SetResourceBuilder(ResourceBuilder.CreateDefault());
            options.AddOtlpExporter(exporter =>
            {
                exporter.ExportProcessorType = openTelemetryConfiguration.ExportProcessorType;
                exporter.Endpoint = new Uri(
                    uriString: openTelemetryConfiguration.Endpoint);
                exporter.Protocol = openTelemetryConfiguration.Protocol;
            });
        });

        return logging;
    }
}
