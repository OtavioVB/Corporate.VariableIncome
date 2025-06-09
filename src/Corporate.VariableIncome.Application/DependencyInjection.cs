using Corporate.VariableIncome.Application.Services.External.B3FinanceAsset;
using Corporate.VariableIncome.Application.Services.External.B3FinanceAsset.Configuration;
using Corporate.VariableIncome.Application.Services.External.B3FinanceAsset.Interfaces;
using Corporate.VariableIncome.Application.Services.Internal.AssetContext;
using Corporate.VariableIncome.Application.Services.Internal.AssetContext.Interfaces;
using Corporate.VariableIncome.Application.Services.Internal.OperationsContext;
using Corporate.VariableIncome.Application.Services.Internal.OperationsContext.Interfaces;
using Corporate.VariableIncome.Application.Services.Internal.PositionContext;
using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Interfaces;
using Corporate.VariableIncome.Application.Services.Internal.QuotationContext;
using Corporate.VariableIncome.Application.Services.Internal.QuotationContext.Interfaces;
using Corporate.VariableIncome.Application.Services.Internal.UserContext;
using Corporate.VariableIncome.Application.Services.Internal.UserContext.Interfaces;
using Corporate.VariableIncome.Application.UseCases.Interfaces;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Inputs;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Outputs;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Builders;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Strategies.Builders.Interfaces;
using Corporate.VariableIncome.Application.UseCases.OperationContext.GetUserOperations;
using Corporate.VariableIncome.Application.UseCases.OperationContext.GetUserOperations.Inputs;
using Corporate.VariableIncome.Application.UseCases.OperationContext.GetUserOperations.Outputs;
using Corporate.VariableIncome.Application.UseCases.PositionContext.GetActualPosition.Inputs;
using Corporate.VariableIncome.Application.UseCases.PositionContext.GetUserActualPosition;
using Corporate.VariableIncome.Application.UseCases.PositionContext.RequestUpdatePosition;
using Corporate.VariableIncome.Application.UseCases.PositionContext.RequestUpdatePosition.Inputs;
using Corporate.VariableIncome.Application.UseCases.PositionContext.UpdatePosition;
using Corporate.VariableIncome.Application.UseCases.PositionContext.UpdatePosition.Inputs;
using Corporate.VariableIncome.Application.UseCases.QuotationContext.UpdateQuotationRealtime;
using Corporate.VariableIncome.Application.UseCases.QuotationContext.UpdateQuotationRealtime.Inputs;
using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.QuotationContext.Entities;
using Corporate.VariableIncome.Domain.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corporate.VariableIncome.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IQuotationService, QuotationService>();
        services.AddScoped<IOperationService, OperationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPositionService, PositionService>();
        services.AddScoped<IAssetService, AssetService>();

        return services;
    }

    public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        var b3FinanceAssetConfiguration = configuration.GetRequiredSection(nameof(B3FinanceAssetConfiguration)).Get<B3FinanceAssetConfiguration>()!;

        services.AddSingleton<IB3FinanceAssetService, B3FinanceAssetService>((serviceProvider)
            => new B3FinanceAssetService(
                logger: serviceProvider.GetRequiredService<ILogger<B3FinanceAssetService>>(),
                configuration: b3FinanceAssetConfiguration));

        return services;
    }

    public static IServiceCollection AddStrategies(this IServiceCollection services)
    {
        services.AddScoped<IOperationStrategyBuilder, OperationStrategyBuilder>();

        return services;
    }

    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IUseCase<CreateOperationUseCaseInput, Result<CreateOperationUseCaseOutput>>, CreateOperationUseCase>();
        services.AddScoped<IUseCase<UpdateQuotationRealtimeUseCaseInput, Result<Quotation>>, UpdateQuotationRealtimeUseCase>();
        services.AddScoped<IUseCase<RequestUpdatePositionUseCaseInput, Result>, RequestUpdatePositionUseCase>();
        services.AddScoped<IUseCase<UpdatePositionUseCaseInput, Result>, UpdatePositionUseCase>();
        services.AddScoped<IUseCase<GetAssetUserOperationsUseCaseInput, Result<GetAssetUserOperationsUseCaseOutput>>, GetUserAssetOperationsUseCase>();
        services.AddScoped<IUseCase<GetUserActualPositionUseCaseInput, Result<PositionSnapshot>>, GetUserActualPositionUseCase>();

        return services;
    }
}
