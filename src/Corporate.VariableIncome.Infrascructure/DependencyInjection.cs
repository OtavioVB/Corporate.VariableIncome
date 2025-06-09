using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.QuotationContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.UserContext.Entities;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Configuration;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Base.Interfaces;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.UnitOfWork;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Corporate.VariableIncome.Infrascructure;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrascructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var mysqlConfiguration = configuration.GetRequiredSection(nameof(PostgreeSQLConfiguration)).Get<PostgreeSQLConfiguration>()!;

        services.AddDbContextPool<DataContext>(
            optionsAction: p => p.UseNpgsql(
                connectionString: mysqlConfiguration.ConnectionString,
                npgsqlOptionsAction: p => p.MigrationsAssembly("Corporate.VariableIncome.Infrascructure")));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseRepository<User>, UserRepository>();
        services.AddScoped<IExtensionUserRepository, UserRepository>();

        services.AddScoped<IBaseRepository<Asset>, AssetRepository>();
        services.AddScoped<IExtensionAssetRepository, AssetRepository>();

        services.AddScoped<IBaseRepository<Quotation>, QuotationRepository>();
        services.AddScoped<IExtensionQuotationRepository, QuotationRepository>();

        services.AddScoped<IBaseRepository<Operation>, OperationRepository>();
        services.AddScoped<IExtensionOperationRepository, OperationRepository>();

        services.AddScoped<IBaseRepository<PositionSnapshot>, PositionSnapshotRepository>();
        services.AddScoped<IExtensionPositionSnapshotRepository, PositionSnapshotRepository>();

        return services;
    }

    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, DefaultUnitOfWork>();

        return services;
    }
}
