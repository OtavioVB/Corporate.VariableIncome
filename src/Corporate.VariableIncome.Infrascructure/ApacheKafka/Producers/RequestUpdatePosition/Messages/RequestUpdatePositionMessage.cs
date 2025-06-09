namespace Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.RequestUpdatePosition.Messages;

public sealed record RequestUpdatePositionMessage
{
    public RequestUpdatePositionMessage(Guid assetId, Guid quotationId, decimal unitaryPrice, DateTime dateTime)
    {
        AssetId = assetId;
        QuotationId = quotationId;
        UnitaryPrice = unitaryPrice;
        DateTime = dateTime;
    }

    public Guid AssetId { get; set; }
    public Guid QuotationId { get; set; }
    public decimal UnitaryPrice { get; set; }
    public DateTime DateTime { get; set; }
}
