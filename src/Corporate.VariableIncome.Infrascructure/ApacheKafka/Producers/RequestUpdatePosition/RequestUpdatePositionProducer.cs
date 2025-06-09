using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.Base;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.Configuration;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.RequestUpdatePosition.Messages;
using Microsoft.Extensions.Logging;

namespace Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.RequestUpdatePosition;

public sealed class RequestUpdatePositionProducer : RetriableProducer<RequestUpdatePositionMessage>
{
    private const string TOPIC_NAME = "corporate-variableincome.positions.update-request";

    public RequestUpdatePositionProducer(
        ILogger<RetriableProducer<RequestUpdatePositionMessage>> logger, 
        ProducerConfiguration configuration) : base(logger, configuration, TOPIC_NAME)
    {
    }
}
