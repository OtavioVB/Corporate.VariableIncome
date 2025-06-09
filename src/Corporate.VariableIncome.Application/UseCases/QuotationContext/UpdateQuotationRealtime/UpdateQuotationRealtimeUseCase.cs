using Corporate.VariableIncome.Application.Services.Internal.AssetContext.Interfaces;
using Corporate.VariableIncome.Application.Services.Internal.QuotationContext.Inputs;
using Corporate.VariableIncome.Application.Services.Internal.QuotationContext.Interfaces;
using Corporate.VariableIncome.Application.UseCases.Interfaces;
using Corporate.VariableIncome.Application.UseCases.QuotationContext.UpdateQuotationRealtime.Inputs;
using Corporate.VariableIncome.Domain.BoundedContexts.QuotationContext.Entities;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.Base.Interfaces;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.RequestUpdatePosition.Messages;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace Corporate.VariableIncome.Application.UseCases.QuotationContext.UpdateQuotationRealtime;

public sealed class UpdateQuotationRealtimeUseCase : IUseCase<UpdateQuotationRealtimeUseCaseInput, Result<Quotation>>
{
    private readonly ILogger<UpdateQuotationRealtimeUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQuotationService _quotationService;
    private readonly IAssetService _assetService;
    private readonly IRetriableProducer<RequestUpdatePositionMessage> _producer;

    public UpdateQuotationRealtimeUseCase(
        ILogger<UpdateQuotationRealtimeUseCase> logger,
        IUnitOfWork unitOfWork, 
        IQuotationService quotationService,
        IAssetService assetService,
        IRetriableProducer<RequestUpdatePositionMessage> producer)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _quotationService = quotationService;
        _assetService = assetService;
        _producer = producer;
    }

    public Task<Result<Quotation>> ExecuteUseCaseAsync(UpdateQuotationRealtimeUseCaseInput input, CancellationToken cancellationToken)
    {
        try
        {
            return _unitOfWork.ExecuteUnitOfWorkAsync(
                input: input,
                handler: async (input, cancellationToken) =>
                {
                    var getUpdateQuotationAssetElegibilityResult = await _assetService.GetAssetUpdateQuotationRealtimeElegibilityAsync(
                        assetId: input.AssetId,
                        cancellationToken: cancellationToken);

                    if (getUpdateQuotationAssetElegibilityResult.IsFailed)
                        return (Commit: false, Result<Quotation>.Error(getUpdateQuotationAssetElegibilityResult.GetRequiredMessage()));

                    var appendQuotationServiceResult = await _quotationService.AppendAssetQuotationAsync(
                        input: AppendAssetQuotationServiceInput.Build(
                            quotationId: input.IdempotencyKey,
                            assetId: input.AssetId,
                            unitaryPrice: input.UnitaryPrice,
                            dateTime: input.DateTime),
                        cancellationToken: cancellationToken);

                    if (appendQuotationServiceResult.IsFailed)
                        return (Commit: false, Result<Quotation>.Error(appendQuotationServiceResult.GetRequiredMessage()));

                    var quotation = appendQuotationServiceResult.GetValue();

                    await _producer.ProduceRetriableAsync(
                        key: quotation.AssetId.GetValueAsString(),
                        message: new RequestUpdatePositionMessage(
                            assetId: quotation.AssetId,
                            quotationId: quotation.Id,
                            unitaryPrice: quotation.UnitaryPrice,
                            dateTime: quotation.DateTime),
                        cancellationToken: cancellationToken);

                    _logger?.LogInformation(
                        message: "[{Type}][{Method}] The update quotation realtime has been appended with successfull. Quotation = {@Quotation}",
                            nameof(UpdateQuotationRealtimeUseCase),
                            nameof(ExecuteUseCaseAsync),
                            new
                            {
                                IdempotencyKey = input.IdempotencyKey.GetValueAsString(),
                                QuotationId = input.IdempotencyKey.GetValueAsString(),
                                AssetId = input.AssetId.GetValueAsString(),
                                UnitaryPrice = input.UnitaryPrice.GetValue(),
                                DateTime = input.DateTime.GetValue()
                            });

                    return (Commit: true, Result<Quotation>.Success(appendQuotationServiceResult.GetValue()));
                },
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Is not possible to update quotation realtime because unhandled exception has been throwed executing the use case.",
                nameof(UpdateQuotationRealtimeUseCase),
                nameof(ExecuteUseCaseAsync));

            throw;
        }
    }
}
