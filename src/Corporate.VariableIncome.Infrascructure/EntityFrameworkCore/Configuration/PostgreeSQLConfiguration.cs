namespace Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Configuration;

public sealed record PostgreeSQLConfiguration
{
    public string ConnectionString { get; set; } = string.Empty;
}
