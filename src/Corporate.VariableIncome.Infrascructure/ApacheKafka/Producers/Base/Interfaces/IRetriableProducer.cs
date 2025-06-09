namespace Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.Base.Interfaces;

public interface IRetriableProducer<T>
{
    public Task ProduceRetriableAsync(string key, T message, CancellationToken cancellationToken = default);
}
