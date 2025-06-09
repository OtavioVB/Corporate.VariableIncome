namespace Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.UpdatePosition.Messages;

public sealed record UpdatePositionMessage
{
    public UpdatePositionMessage(UpdatePositionMessageQuotation quotation, UpdatePositionMessagePosition latestPosition)
    {
        Quotation = quotation;
        LatestPosition = latestPosition;
    }

    public UpdatePositionMessageQuotation Quotation { get; set; }
    public UpdatePositionMessagePosition LatestPosition { get; set; }
}

public sealed record UpdatePositionMessagePosition
{
    public UpdatePositionMessagePosition(Guid assetId, Guid userId, int quantity, decimal averagePrice, DateTime dateTime, decimal proftAndLoss)
    {
        AssetId = assetId;
        UserId = userId;
        Quantity = quantity;
        AveragePrice = averagePrice;
        DateTime = dateTime;
        ProftAndLoss = proftAndLoss;
    }

    public Guid AssetId { get; set; }
    public Guid UserId { get; set; }
    public int Quantity { get; set; }
    public decimal AveragePrice { get; set; }
    public DateTime DateTime { get; set; }
    public decimal ProftAndLoss { get; set; }
}

public sealed record UpdatePositionMessageQuotation
{
    public UpdatePositionMessageQuotation(Guid assetId, Guid quotationId, decimal unitaryPrice, DateTime dateTime)
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