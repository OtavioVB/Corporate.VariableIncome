namespace Corporate.VariableIncome.Application.Services.External.B3FinanceAsset.Outputs;

public sealed record GetFinanceAssetQuotationServiceOutput
{
    public GetFinanceAssetQuotationServiceOutput(decimal price, DateTime tradetime)
    {
        this.price = price;
        this.tradetime = tradetime;
    }

    public decimal price { get; set; }
    public DateTime tradetime { get; set; }
}
