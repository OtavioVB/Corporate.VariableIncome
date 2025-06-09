
using Confluent.Kafka;
using Corporate.VariableIncome.Application.UseCases.Interfaces;
using Corporate.VariableIncome.Application.UseCases.PositionContext.UpdatePosition.Inputs;
using Corporate.VariableIncome.Application.UseCases.QuotationContext.UpdateQuotationRealtime.Inputs;
using Corporate.VariableIncome.Domain.BoundedContexts.QuotationContext.Entities;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.UpdatePosition.Messages;
using Corporate.VariableIncome.Worker.Consumers.UpdateQuotationRealtime;
using System.Text.Json;

namespace Corporate.VariableIncome.Worker.Consumers.UpdatePosition;

public sealed class UpdatePositionConsumer : BackgroundService
{
    private readonly ILogger<UpdatePositionConsumer> _logger;
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceProvider _serviceProvider;

    public UpdatePositionConsumer(ILogger<UpdatePositionConsumer> logger, ConsumerConfig consumerConfig, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _consumerConfig = consumerConfig;
        _serviceProvider = serviceProvider;
    }

    private const string TOPIC_NAME = "corporate-variableincome.positions.update";

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();

        consumer.Subscribe(TOPIC_NAME);

        _ = Task.Run(async () =>
        {
            _logger.LogInformation("[{Type}][{Method}] The apache kafka consumer has been started. Info = {@Info}",
            nameof(UpdatePositionConsumer),
            nameof(ExecuteAsync),
            new
            {
                EventType = nameof(UpdatePositionConsumer),
                GroupId = _consumerConfig.GroupId,
                TopicName = TOPIC_NAME
            });

            while (!stoppingToken.IsCancellationRequested)
            {
                var scope = _serviceProvider.CreateScope();

                var useCase = scope.ServiceProvider.GetRequiredService<IUseCase<UpdatePositionUseCaseInput, Result>>();

                var @event = consumer.Consume(stoppingToken);

                var message = JsonSerializer.Deserialize<UpdatePositionMessage>(@event.Message.Value, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                })!;

                _logger.LogInformation("[{Type}][{Method}] The event has been received. Event = {@Event}",
                    nameof(UpdatePositionConsumer),
                    nameof(ExecuteAsync),
                    new
                    {
                        Topic = TOPIC_NAME,
                        Key = @event.Message.Key,
                        Value = @event.Message.Value,
                        Offset = @event.Offset.Value,
                        Partition = @event.TopicPartition.Partition.Value
                    });

                var useCaseInput = UpdatePositionUseCaseInput.Factory(
                    position: UpdatePositionUseCaseInputPosition.Factory(
                        assetId: message.LatestPosition.AssetId,
                        userId: message.LatestPosition.UserId,
                        quantity: message.LatestPosition.Quantity,
                        averagePrice: message.LatestPosition.AveragePrice,
                        dateTime: message.LatestPosition.DateTime,
                        proftAndLoss: message.LatestPosition.ProftAndLoss),
                    quotation: UpdatePositionUseCaseInputQuotation.Factory(
                        assetId: message.Quotation.AssetId,
                        quotationId: message.Quotation.QuotationId,
                        unitaryPrice: message.Quotation.UnitaryPrice,
                        dateTime: message.Quotation.DateTime));

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
