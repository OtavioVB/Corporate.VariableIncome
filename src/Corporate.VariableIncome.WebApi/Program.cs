using Corporate.VariableIncome.Application;
using Corporate.VariableIncome.Infrascructure;
using Corporate.VariableIncome.Infrascructure.ApacheKafka;
using Corporate.VariableIncome.Infrascructure.OpenTelemetry;

namespace Corporate.VariableIncome.WebApi;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddOpenTelemetryConfiguration(builder.Configuration);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services
            .AddApacheKafkaProducers(builder.Configuration)
            .AddInfrascructure(builder.Configuration)
            .AddExternalServices(builder.Configuration)
            .AddRepositories()
            .AddUnitOfWork()
            .AddDomainServices()
            .AddStrategies()
            .AddUseCases();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapControllers();
        app.Run();
    }
}
