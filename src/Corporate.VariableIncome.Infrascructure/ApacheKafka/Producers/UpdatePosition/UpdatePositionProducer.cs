using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.Base;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.Configuration;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.UpdatePosition.Messages;
using Microsoft.Extensions.Logging;

namespace Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.UpdatePosition;

public sealed class UpdatePositionProducer : RetriableProducer<UpdatePositionMessage>
{
    private const string TOPIC_NAME = "corporate-variableincome.positions.update";

    public UpdatePositionProducer(
        ILogger<RetriableProducer<UpdatePositionMessage>> logger, 
        ProducerConfiguration configuration) : base(logger, configuration, TOPIC_NAME)
    {
    }
}
