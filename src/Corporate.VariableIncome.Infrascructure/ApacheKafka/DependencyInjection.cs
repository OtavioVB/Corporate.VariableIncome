using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.Base;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.Base.Interfaces;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.Configuration;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.RequestUpdatePosition;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.RequestUpdatePosition.Messages;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.UpdatePosition;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.UpdatePosition.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corporate.VariableIncome.Infrascructure.ApacheKafka;

public static class DependencyInjection
{
    public static IServiceCollection AddApacheKafkaProducers(this IServiceCollection services, IConfiguration configuration)
    {
        var producerConfig = configuration.GetRequiredSection(nameof(ProducerConfiguration)).Get<ProducerConfiguration>()!;

        services.AddSingleton<IRetriableProducer<RequestUpdatePositionMessage>, RequestUpdatePositionProducer>((serviceProvider)
            => new RequestUpdatePositionProducer(
                logger: serviceProvider.GetRequiredService<ILogger<RetriableProducer<RequestUpdatePositionMessage>>>(),
                configuration: producerConfig));

        services.AddSingleton<IRetriableProducer<UpdatePositionMessage>, UpdatePositionProducer>((serviceProvider)
            => new UpdatePositionProducer(
                logger: serviceProvider.GetRequiredService<ILogger<RetriableProducer<UpdatePositionMessage>>>(),
                configuration: producerConfig));

        return services;
    }
}
