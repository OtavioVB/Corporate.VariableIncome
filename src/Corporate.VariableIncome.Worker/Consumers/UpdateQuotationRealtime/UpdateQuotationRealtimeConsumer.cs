using Confluent.Kafka;
using Corporate.VariableIncome.Application.UseCases.Interfaces;
using Corporate.VariableIncome.Application.UseCases.QuotationContext.UpdateQuotationRealtime.Inputs;
using Corporate.VariableIncome.Domain.BoundedContexts.QuotationContext.Entities;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Worker.Consumers.UpdateQuotationRealtime.Message;
using System.Text.Json;

namespace Corporate.VariableIncome.Worker.Consumers.UpdateQuotationRealtime;

public sealed class UpdateQuotationRealtimeConsumer : BackgroundService
{
    private readonly ILogger<UpdateQuotationRealtimeConsumer> _logger;
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceProvider _serviceProvider;

    public UpdateQuotationRealtimeConsumer(
        ILogger<UpdateQuotationRealtimeConsumer> logger,
        ConsumerConfig consumerConfig,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _consumerConfig = consumerConfig;
        _serviceProvider = serviceProvider;
    }

    private const string TOPIC_NAME = "corporate-variableincome.asset-quotation.updated";

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();

        consumer.Subscribe(TOPIC_NAME);

        _ = Task.Run(async () =>
        {
            _logger.LogInformation("[{Type}][{Method}] The apache kafka consumer has been started. Info = {@Info}",
            nameof(UpdateQuotationRealtimeConsumer),
            nameof(ExecuteAsync),
            new
            {
                EventType = nameof(UpdateQuotationRealtimeMessage),
                GroupId = _consumerConfig.GroupId,
                TopicName = TOPIC_NAME
            });

            while (!stoppingToken.IsCancellationRequested)
            {
                var scope = _serviceProvider.CreateScope();

                var useCase = scope.ServiceProvider.GetRequiredService<IUseCase<UpdateQuotationRealtimeUseCaseInput, Result<Quotation>>>();

                var @event = consumer.Consume(stoppingToken);

                var message = JsonSerializer.Deserialize<UpdateQuotationRealtimeMessage>(@event.Message.Value, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                })!;

                _logger.LogInformation("[{Type}][{Method}] The event has been received. Event = {@Event}",
                    nameof(UpdateQuotationRealtimeConsumer),
                    nameof(ExecuteAsync),
                    new
                    {
                        Topic = TOPIC_NAME,
                        Key = @event.Message.Key,
                        Value = @event.Message.Value,
                        Offset = @event.Offset.Value,
                        Partition = @event.TopicPartition.Partition.Value
                    });

                var useCaseInput = UpdateQuotationRealtimeUseCaseInput.Build(
                    idempotencyKey: message.IdempotencyKey,
                    assetId: message.AssetId,
                    unitaryPrice: message.UnitaryPrice,
                    dateTime: message.DateTime);

                var useCaseResult = await useCase.ExecuteUseCaseAsync(
                    input: useCaseInput,
                    cancellationToken: stoppingToken);
            }

            consumer.Close();
            consumer.Dispose();
        }, stoppingToken);

        return Task.CompletedTask;
    }
}
