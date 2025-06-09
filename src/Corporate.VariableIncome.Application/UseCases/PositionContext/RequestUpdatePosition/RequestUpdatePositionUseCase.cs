using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Inputs;
using Corporate.VariableIncome.Application.Services.Internal.PositionContext.Interfaces;
using Corporate.VariableIncome.Application.UseCases.Interfaces;
using Corporate.VariableIncome.Application.UseCases.PositionContext.RequestUpdatePosition.Inputs;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Domain.ValueObjects;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.Base.Interfaces;
using Corporate.VariableIncome.Infrascructure.ApacheKafka.Producers.UpdatePosition.Messages;
using Microsoft.Extensions.Logging;

namespace Corporate.VariableIncome.Application.UseCases.PositionContext.RequestUpdatePosition;

public sealed class RequestUpdatePositionUseCase : IUseCase<RequestUpdatePositionUseCaseInput, Result>
{
    private readonly ILogger<RequestUpdatePositionUseCase> _logger;
    private readonly IPositionService _positionService;
    private readonly IRetriableProducer<UpdatePositionMessage> _producer;

    public RequestUpdatePositionUseCase(ILogger<RequestUpdatePositionUseCase> logger, IPositionService positionService, IRetriableProducer<UpdatePositionMessage> producer)
    {
        _logger = logger;
        _positionService = positionService;
        _producer = producer;
    }

    public async Task<Result> ExecuteUseCaseAsync(
        RequestUpdatePositionUseCaseInput input, 
        CancellationToken cancellationToken)
    {
        try
        {
            var page = PageValueObject.Build(1);
            var offset = OffsetValueObject.Build();
            var hasMorePages = true;

            while (hasMorePages)
            {
                var getUserPositionsByAssetIdServiceResult = await _positionService.GetUserLatestPositionPaginationByAssetServiceAsync(
                    GetUserLatestPositionPaginationByAssetServiceInput.Factory(
                        page: page,
                        offset: offset,
                        assetId: input.AssetId),
                    cancellationToken: cancellationToken);

                if (getUserPositionsByAssetIdServiceResult.IsFailed)
                    return Result.Error(getUserPositionsByAssetIdServiceResult.GetRequiredMessage());

                var result = getUserPositionsByAssetIdServiceResult.GetValue();

                hasMorePages = result.HasMorePages;
                page = page + 1;

                foreach (var position in result.Positions)
                {
                    var message = new UpdatePositionMessage(
                        quotation: new UpdatePositionMessageQuotation(
                            assetId: input.AssetId,
                            quotationId: input.QuotationId,
                            unitaryPrice: input.UnitaryPrice,
                            dateTime: input.DateTime),
                        latestPosition: new UpdatePositionMessagePosition(
                            assetId: input.AssetId,
                            userId: position.UserId,
                            quantity: position.Quantity,
                            averagePrice: position.AveragePrice,
                            dateTime: position.DateTime,
                            proftAndLoss: position.ProftAndLoss));

                    await _producer.ProduceRetriableAsync(
                        key: input.AssetId.GetValueAsString(),
                        message: message,
                        cancellationToken: cancellationToken);
                }
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Is not possible to request update positions by quotation realtime update. Input = {@Input}",
                nameof(RequestUpdatePositionUseCase),
                nameof(ExecuteUseCaseAsync),
                new
                {
                    AssetId = input.AssetId.GetValueAsString(),
                    QuotationId = input.QuotationId.GetValueAsString()
                });

            return Result.Error("Erro no processamento de solicitação de atualização de posições do usuário.");
        }
    }
}
