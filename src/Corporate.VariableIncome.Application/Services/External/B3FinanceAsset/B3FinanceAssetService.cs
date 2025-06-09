using Corporate.VariableIncome.Application.Services.External.B3FinanceAsset.Configuration;
using Corporate.VariableIncome.Application.Services.External.B3FinanceAsset.Interfaces;
using Corporate.VariableIncome.Application.Services.External.B3FinanceAsset.Outputs;
using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.ValueObjects;
using Corporate.VariableIncome.Domain.Helpers;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;

namespace Corporate.VariableIncome.Application.Services.External.B3FinanceAsset;

public sealed class B3FinanceAssetService : IB3FinanceAssetService
{
    private readonly ILogger<B3FinanceAssetService> _logger;
    private readonly B3FinanceAssetConfiguration _configuration;

    public B3FinanceAssetService(ILogger<B3FinanceAssetService> logger, B3FinanceAssetConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<Result<GetFinanceAssetQuotationServiceOutput>> GetFinanceAssetQuotationServiceAsync(AssetCodeValueObject code, CancellationToken cancellationToken)
    {
        try
        {
            if (!code.IsValid)
                return Result<GetFinanceAssetQuotationServiceOutput>.Error(
                    message: "É necessário enviar um códig de ativo financeiro válido para realizar a consulta de sua cotação.");

            var endpoint = $"{_configuration.Host}/api/Assets/{code.GetValue()}";

            using var httpClient = HttpClientFactory.Create();

            var response = await httpClient.GetAsync(
                requestUri: endpoint,
                cancellationToken: cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<GetFinanceAssetQuotationServiceOutput>();

                return Result<GetFinanceAssetQuotationServiceOutput>.Success(content!);
            }

            return Result<GetFinanceAssetQuotationServiceOutput>.Error(
                 message: "Não foi possível fazer a consulta da atual cotação do ativo financeiro.");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Is not possible to get finance asset quotation because unhandled exception has been throwed. Input = {@Input}",
                nameof(B3FinanceAssetService),
                nameof(GetFinanceAssetQuotationServiceAsync),
                new
                {
                    IsValid = code.IsValid,
                    Code = code.IsValid ? code.GetValue() : "INVALID_ASSET_CODE"
                });

            return Result<GetFinanceAssetQuotationServiceOutput>.Error(
                "Não foi possível consultar a cotação atual do ativo financeiro.");
        }
    }
}
