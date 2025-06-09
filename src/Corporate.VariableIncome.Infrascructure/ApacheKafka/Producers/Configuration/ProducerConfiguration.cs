using Confluent.Kafka;

namespace Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.Configuration;

public sealed record ProducerConfiguration
{
    public ProducerConfig ProducerConfig { get; set; } = new ProducerConfig();
    public ProducerConfigurationRetriable Retriable { get; set; } = new ProducerConfigurationRetriable();

}

public sealed record ProducerConfigurationRetriable
{
    public bool IsRetriable { get; set; } = false;
    public int RetryCount { get; set; } = 0;
    public int DelayInMiliseconds { get; set; } = 0;
}