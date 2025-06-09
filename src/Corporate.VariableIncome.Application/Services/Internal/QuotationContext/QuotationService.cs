using Corporate.VariableIncome.Application.Services.Internal.QuotationContext.Inputs;
using Corporate.VariableIncome.Application.Services.Internal.QuotationContext.Interfaces;
using Corporate.VariableIncome.Domain.BoundedContexts.QuotationContext.Entities;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Domain.ValueObjects;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Base.Interfaces;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace Corporate.VariableIncome.Application.Services.Internal.QuotationContext;

public sealed class QuotationService : IQuotationService
{
    private readonly ILogger<QuotationService> _logger;
    private readonly IExtensionQuotationRepository _extensionQuotationRepository;
    private readonly IBaseRepository<Quotation> _baseQuotationRepository;
    private readonly IUnitOfWork _unitOfWork;


    public QuotationService(
        ILogger<QuotationService> logger,
        IExtensionQuotationRepository extensionQuotationRepository,
        IBaseRepository<Quotation> baseQuotationRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _extensionQuotationRepository = extensionQuotationRepository;
        _baseQuotationRepository = baseQuotationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Quotation>> AppendAssetQuotationAsync(AppendAssetQuotationServiceInput input, CancellationToken cancellationToken)
    {
        try
        {
            if (!input.IsValid)
                return Result<Quotation>.Error(
                    message: "É necessário informar dados de cotação de ativo válidos para atualizar a cotação.");

            var quotation = await _extensionQuotationRepository.QueryAssetQuotationByIdAsNoTrackingAsync(
                quotationId: input.QuotationId,
                cancellationToken: cancellationToken);

            if (quotation != null)
                return Result<Quotation>.Success(quotation);

            var appendQuotation = new Quotation(
                id: input.QuotationId,
                unitaryPrice: input.UnitaryPrice,
                dateTime: input.DateTime);
            appendQuotation.AssetId = input.AssetId;

            await _baseQuotationRepository.AddAsync(appendQuotation, cancellationToken);
            await _unitOfWork.ApplyTransactionAsync(cancellationToken);

            return Result<Quotation>.Success(appendQuotation);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Is not possible to append asset quotation because unhandled exception has been throwed. Input = {@Input}",
                nameof(QuotationService),
                nameof(AppendAssetQuotationAsync),
                new
                {
                    AssetId = input.AssetId.GetValueAsString(),
                    QuotationId = input.QuotationId.GetValueAsString(),
                    IdempotencyKey = input.QuotationId.GetValueAsString(),
                    UnitaryPrice = input.UnitaryPrice.GetValue(),
                    DateTime = input.DateTime.GetValue()
                });
            throw;
        }
    }

    public async Task<Result<Quotation>> QueryLatestAssetQuotationAsync(IdValueObject assetId, CancellationToken cancellationToken)
    {
        try
        {
            if (!assetId.IsValid)
                return Result<Quotation>.Error(
                    message: "ID de identificação do ativo financeiro precisa ser válido.");

            var quotation = await _extensionQuotationRepository.QueryLatestAssetQuotationAsNoTrackingAsync(
                assetId: assetId,
                cancellationToken: cancellationToken);

            if (quotation == null)
            {
                return Result<Quotation>.Error(
                    message: "Não foi possível encontrar nenhuma cotação para o ativo financeiro atual.");
            }

            return Result<Quotation>.Success(quotation);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Is not possible to query latest asset quotation, because unhandled exception has been throwed. Asset = {@Input}",
                nameof(QuotationService),
                nameof(QueryLatestAssetQuotationAsync),
                new
                {
                    AssetId = assetId
                });

            return Result<Quotation>.Error(
                message: "Um erro inesperado ocorreu ao consultar a cotação para o ativo financeiro.");
        }
    }
}
