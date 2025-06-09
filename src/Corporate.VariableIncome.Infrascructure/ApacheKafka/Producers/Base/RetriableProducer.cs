using Confluent.Kafka;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.Base.Interfaces;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Text.Json;

namespace Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.Base;

public class RetriableProducer<T> : IRetriableProducer<T>
{
    protected readonly ProducerConfiguration _configuration;
    protected readonly ILogger<RetriableProducer<T>> _logger;
    protected readonly IProducer<string, string> _producer;
    protected readonly string _topicName;

    public RetriableProducer(
        ILogger<RetriableProducer<T>> logger,
        ProducerConfiguration producerConfiguration,
        string topicName)
    {
        _logger = logger;
        _producer = new ProducerBuilder<string, string>(producerConfiguration.ProducerConfig).Build();
        _configuration = producerConfiguration;
        _topicName = topicName;
    }

    protected virtual async Task ProduceAsync(string key, T message, CancellationToken cancellationToken = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(message);

            var @event = new Message<string, string>()
            {
                Key = key,
                Value = json
            };

            var result = await _producer.ProduceAsync(
                topic: _topicName,
                message: @event,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] An unhandled exception has been throwed producing apache kafka message. Info = {@Info}",
                nameof(RetriableProducer<T>),
                nameof(ProduceAsync),
                new
                {
                    Key = key,
                });
        }
    }

    public async Task ProduceRetriableAsync(string key, T message, CancellationToken cancellationToken)
    {
        if (!_configuration.Retriable.IsRetriable)
        {
            await ProduceAsync(key, message, cancellationToken);
            return;
        }

        var retriableConfiguration = _configuration.Retriable;

        try
        {
            AsyncRetryPolicy retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: retriableConfiguration.RetryCount,
                    sleepDurationProvider: _ => TimeSpan.FromMilliseconds(retriableConfiguration.DelayInMiliseconds),
                    onRetry: (exception, delay, retryCount, context) =>
                    {
                        _logger?.LogWarning(
                            exception: exception,
                            "[{Type}][{Method}] Try {Retry} has been failed. Resending by {Delay}ms. RetriableConfiguration = {@RetriableConfiguration}",
                            nameof(RetriableProducer<T>),
                            nameof(ProduceRetriableAsync),
                            retryCount,
                            delay,
                            retriableConfiguration);
                    });

            await retryPolicy.ExecuteAsync(async () =>
            {
                var json = JsonSerializer.Serialize(message);

                var @event = new Message<string, string>()
                {
                    Key = key,
                    Value = json
                };

                var result = await _producer.ProduceAsync(_topicName, @event, cancellationToken);
            });
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] All retries has been failed. RetriableConfiguration = {@RetriableConfiguration}; Key = {Key};",
                nameof(RetriableProducer<T>),
                nameof(ProduceRetriableAsync),
                retriableConfiguration,
                key);
        }
    }
}
