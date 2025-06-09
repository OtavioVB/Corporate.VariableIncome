using Confluent.Kafka;
using Corporate.VariableIncome.Application;
using Corporate.VariableIncome.Infrascructure;
using Corporate.VariableIncome.Infrascructure.ApacheKafka;
using Corporate.VariableIncome.Infrascructure.OpenTelemetry;
using Corporate.VariableIncome.Worker.Consumers.RequestUpdatePosition;
using Corporate.VariableIncome.Worker.Consumers.UpdatePosition;
using Corporate.VariableIncome.Worker.Consumers.UpdateQuotationRealtime;

namespace Corporate.VariableIncome.Worker;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Logging.AddOpenTelemetryConfiguration(builder.Configuration);

        builder.Services
            .AddApacheKafkaProducers(builder.Configuration)
            .AddInfrascructure(builder.Configuration)
            .AddExternalServices(builder.Configuration)
            .AddRepositories()
            .AddUnitOfWork()
            .AddDomainServices()
            .AddStrategies()
            .AddUseCases();

        builder.Services.AddHostedService((serviceProvider) => 
            new UpdateQuotationRealtimeConsumer(
                logger: serviceProvider.GetRequiredService<ILogger<UpdateQuotationRealtimeConsumer>>(),
                consumerConfig: builder.Configuration.GetRequiredSection(nameof(ConsumerConfig)).Get<ConsumerConfig>()!,
                serviceProvider: serviceProvider));
        builder.Services.AddHostedService((serviceProvider) =>
            new RequestUpdatePositionConsumer(
                logger: serviceProvider.GetRequiredService<ILogger<RequestUpdatePositionConsumer>>(),
                consumerConfig: builder.Configuration.GetRequiredSection(nameof(ConsumerConfig)).Get<ConsumerConfig>()!,
                serviceProvider: serviceProvider));
        builder.Services.AddHostedService((serviceProvider) =>
            new UpdatePositionConsumer(
                logger: serviceProvider.GetRequiredService<ILogger<UpdatePositionConsumer>>(),
                consumerConfig: builder.Configuration.GetRequiredSection(nameof(ConsumerConfig)).Get<ConsumerConfig>()!,
                serviceProvider: serviceProvider));

        var host = builder.Build();

        host.Run();
    }
}