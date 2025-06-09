using Corporate.VariableIncome.Application.Services.Internal.AssetContext.Interfaces;
using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.ValueObjects;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Domain.ValueObjects;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using Microsoft.Extensions.Logging;

namespace Corporate.VariableIncome.Application.Services.Internal.AssetContext;

public sealed class AssetService : IAssetService
{
    private readonly ILogger<AssetService> _logger;
    private readonly IExtensionAssetRepository _extensionAssetRepository;

    public AssetService(ILogger<AssetService> logger, IExtensionAssetRepository extensionAssetRepository)
    {
        _logger = logger;
        _extensionAssetRepository = extensionAssetRepository;
    }

    public async Task<Result<Asset>> GetAssetByCodeAsync(AssetCodeValueObject code, CancellationToken cancellationToken)
    {
        try
        {
            if (!code.IsValid)
                return Result<Asset>.Error("O código do ativo financeiro precisa ser válido.");

            var asset = await _extensionAssetRepository.GetAssetByCodeAsNoTrackingAsync(
                code: code,
                cancellationToken: cancellationToken);

            if (asset is null)
                return Result<Asset>.Error("Não foi possível encontrar o ativo financeiro disponível na corretora.");

            return Result<Asset>.Success(asset);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] An unhandled exception has been throwed querying asset by code. Input = {@Input}",
                nameof(AssetService),
                nameof(GetAssetByCodeAsync),
                new
                {
                    code.IsValid,
                    Code = code.IsValid ? code.GetValue() : "INVALID_ASSET_CODE"
                });

            return Result<Asset>.Error(
                message: "O código do ativo financeiro informado não foi possível ser consultado por erro inesperado.");
        }
    }

    public async Task<Result<Asset>> GetAssetCreateOperationElegibilityAsync(IdValueObject assetId, CancellationToken cancellationToken)
    {
        if (!assetId.IsValid)
            return Result<Asset>.Error(
                message: "É necessário enviar um ID de ativo financeiro válido.");

        var asset = await _extensionAssetRepository.GetAssetAsNoTrackingByIdAsync(
            assetId: assetId,
            cancellationToken: cancellationToken);

        if (asset == null)
            return Result<Asset>.Error(
                message: "Não foi possível encontrar um ativo financeiro pelo ID enviado.");

        /*
        * Alguma regra de elegibilidade, como status do ativo financeiro, allow list, block list, etc
        */

        return Result<Asset>.Success(
            value: asset);
    }

    public async Task<Result> GetAssetUpdateQuotationRealtimeElegibilityAsync(IdValueObject assetId, CancellationToken cancellationToken)
    {
        try
        {
            if (!assetId.IsValid)
                return Result.Error(
                    message: "É necessário enviar um ID de ativo financeiro válido.");

            var assetExists = await _extensionAssetRepository.VerifyAssetExistsByIdAsync(
                assetId: assetId,
                cancellationToken: cancellationToken);

            if (!assetExists)
                return Result.Error(
                    message: "Não foi possível encontrar um ativo financeiro pelo ID enviado.");

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Is not possible to get asset update quotation realtime elegibility because unhandled exception has been throwed by application. AssetId = {AssetId}",
                nameof(AssetService),
                nameof(GetAssetUpdateQuotationRealtimeElegibilityAsync),
                assetId.GetValueAsString());

            throw;
        }
    }
}
