namespace Corporate.VariableIncome.Worker.Consumers.UpdateQuotationRealtime.Message;

public sealed class UpdateQuotationRealtimeMessage
{
    public UpdateQuotationRealtimeMessage(Guid idempotencyKey, Guid assetId, decimal unitaryPrice, DateTime dateTime)
    {
        IdempotencyKey = idempotencyKey;
        AssetId = assetId;
        UnitaryPrice = unitaryPrice;
        DateTime = dateTime;
    }

    public Guid IdempotencyKey { get; set; }
    public Guid AssetId { get; set; }
    public decimal UnitaryPrice { get; set; }
    public DateTime DateTime { get; set; }
}
