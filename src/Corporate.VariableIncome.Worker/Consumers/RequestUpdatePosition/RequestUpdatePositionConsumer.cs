using Confluent.Kafka;
using Corporate.VariableIncome.Application.UseCases.Interfaces;
using Corporate.VariableIncome.Application.UseCases.PositionContext.RequestUpdatePosition.Inputs;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.RequestUpdatePosition.Messages;
using Corporate.VariableIncome.Worker.Consumers.UpdateQuotationRealtime;
using System.Text.Json;

namespace Corporate.VariableIncome.Worker.Consumers.RequestUpdatePosition;

public sealed class RequestUpdatePositionConsumer : BackgroundService
{
    private readonly ILogger<RequestUpdatePositionConsumer> _logger;
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceProvider _serviceProvider;

    public RequestUpdatePositionConsumer(
        ILogger<RequestUpdatePositionConsumer> logger, 
        ConsumerConfig consumerConfig, 
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _consumerConfig = consumerConfig;
        _serviceProvider = serviceProvider;
    }

    private const string TOPIC_NAME = "corporate-variableincome.positions.update-request";

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();

        consumer.Subscribe(TOPIC_NAME);

        _ = Task.Run(async () =>
        {
            _logger.LogInformation("[{Type}][{Method}] The apache kafka consumer has been started. Info = {@Info}",
            nameof(RequestUpdatePositionConsumer),
            nameof(ExecuteAsync),
            new
            {
                EventType = nameof(RequestUpdatePositionConsumer),
                GroupId = _consumerConfig.GroupId,
                TopicName = TOPIC_NAME
            });

            while (!stoppingToken.IsCancellationRequested)
            {
                var scope = _serviceProvider.CreateScope();

                var useCase = scope.ServiceProvider.GetRequiredService<IUseCase<RequestUpdatePositionUseCaseInput, Result>>();

                var @event = consumer.Consume(stoppingToken);

                var message = JsonSerializer.Deserialize<RequestUpdatePositionMessage>(@event.Message.Value, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                })!;

                _logger.LogInformation("[{Type}][{Method}] The event has been received. Event = {@Event}",
                    nameof(RequestUpdatePositionConsumer),
                    nameof(ExecuteAsync),
                    new
                    {
                        Topic = TOPIC_NAME,
                        Key = @event.Message.Key,
                        Value = @event.Message.Value,
                        Offset = @event.Offset.Value,
                        Partition = @event.TopicPartition.Partition.Value
                    });

                await useCase.ExecuteUseCaseAsync(
                    input: RequestUpdatePositionUseCaseInput.Factory(
                        quotationId: message.QuotationId,
                        assetId: message.AssetId,
                        unitaryPrice: message.UnitaryPrice,
                        dateTime: message.DateTime),
                    cancellationToken: stoppingToken);
            }

            consumer.Close();
            consumer.Dispose();
        }, stoppingToken);

        return Task.CompletedTask;
    }
}
