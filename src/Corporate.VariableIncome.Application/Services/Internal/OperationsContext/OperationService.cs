using Corporate.VariableIncome.Application.Services.Internal.OperationsContext.Inputs;
using Corporate.VariableIncome.Application.Services.Internal.OperationsContext.Interfaces;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.ValueObjects;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Domain.ValueObjects;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Base.Interfaces;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace Corporate.VariableIncome.Application.Services.Internal.OperationsContext;

public sealed class OperationService : IOperationService
{
    private readonly ILogger<OperationService> _logger;
    private readonly IBaseRepository<Operation> _operationBaseRepository;
    private readonly IExtensionOperationRepository _extensionOperationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OperationService(
        ILogger<OperationService> logger,
        IBaseRepository<Operation> operationBaseRepository,
        IExtensionOperationRepository extensionOperationRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _operationBaseRepository = operationBaseRepository;
        _extensionOperationRepository = extensionOperationRepository;
        _unitOfWork = unitOfWork;
    }

    /*
     * Comentários de Intenção:
     * 
     * Serviços de domínio são destinados a encapsular operações de negócio que exigem a ponte entre o domínio do negócio e dependências 
     * externas (infrascructure). Nesse caso, realizamos a operação de validação inicial da operação do serviço de domínio, e caso,
     * seja válido, realizamos as operações relacionadas, como a criação de operação de compra de ativo financeiro.
     */
    public async Task<Result<Operation>> CreateBuyOperationServiceAsync(
        CreateBuyOperationServiceInput input,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!input.IsValid)
                return Result<Operation>.Error(
                    message: input.ErrorMessage);

            var operation = new Operation(
                id: IdValueObject.Build(),
                unitaryPrice: input.UnitaryPrice,
                quantity: input.Quantity,
                brokerageFee: input.BrokerageFee,
                dateTime: DateTimeValueObject.Build(),
                type: TypeOperationValueObject.BUY);
            operation.AssetId = input.AssetId;
            operation.UserId = input.UserId;

            await _operationBaseRepository.AddAsync(operation, cancellationToken);
            await _unitOfWork.ApplyTransactionAsync(cancellationToken);

            return Result<Operation>.Success(operation);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}] Is not possible to create buy operation, because an unhandled exception has been throwed. Input = {@Input}",
                nameof(CreateBuyOperationServiceAsync),
                new
                {
                    input.IsValid,
                    AssetId = input.AssetId.IsValid ? input.AssetId.GetValueAsString() : null,
                    UserId = input.UserId.IsValid ? input.UserId.GetValueAsString() : null,
                    Quantity = input.Quantity.IsValid ? input.Quantity.GetValue() : 0
                });

            return Result<Operation>.Error(
                message: "Não é possível criar uma operação de compra de ativo financeiro.");
        }
    }

    public async Task<Result<Operation>> CreateSellOperationServiceAsync(
        CreateSellOperationServiceInput input,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!input.IsValid)
                return Result<Operation>.Error(
                    message: "É necessário enviar dados válidos para criar uma operação de venda de ativo financeiro.");

            var operation = new Operation(
                    id: IdValueObject.Build(),
                    unitaryPrice: input.UnitaryPrice,
                    quantity: input.Quantity,
                    brokerageFee: input.BrokerageFee,
                    dateTime: DateTimeValueObject.Build(),
                    type: TypeOperationValueObject.SELL);
            operation.AssetId = input.AssetId;
            operation.UserId = input.UserId;

            await _operationBaseRepository.AddAsync(operation, cancellationToken);
            await _unitOfWork.ApplyTransactionAsync(cancellationToken);

            return Result<Operation>.Success(operation);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}] Is not possible to create sell operation, because an unhandled exception has been throwed. Input = {@Input}",
                nameof(CreateBuyOperationServiceAsync),
                new
                {
                    input.IsValid,
                    AssetId = input.AssetId.IsValid ? input.AssetId.GetValueAsString() : null,
                    UserId = input.UserId.IsValid ? input.UserId.GetValueAsString() : null,
                    Quantity = input.Quantity.IsValid ? input.Quantity.GetValue() : 0
                });

            return Result<Operation>.Error(
                message: "Não é possível criar uma operação de venda de ativo financeiro.");
        }
    }

    public async Task<Result<Operation[]>> GetUserAssetOperationsServiceAsync(
        GetUserAssetOperationsServiceInput input,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!input.IsValid)
                return Result<Operation[]>.Error("É necessário enviar dados válidos para consulta as operações de um usuário em um ativo financeiro.");

            if (!input.IsValidStartDateLessThanEndDate)
                return Result<Operation[]>.Error("É necessário que a data de ínicio da consulta seja menor que a data de fim.");

            var operations = await _extensionOperationRepository.GetUserAssetOperationsAsync(
                assetId: input.AssetId,
                userId: input.UserId,
                startDate: input.StartDate,
                endDate: input.EndDate,
                page: input.Page,
                offset: input.Offset,
                cancellationToken: cancellationToken);

            return Result<Operation[]>.Success(operations);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Is not possible to get user asset operations, because an unhandled exception has been thwored. Input = {@Input}",
                nameof(OperationService),
                nameof(GetUserAssetOperationsServiceAsync),
                new
                {
                    input.IsValid,
                    AssetId = input.AssetId.GetValue(),
                    UserId = input.UserId.GetValue(),
                    StartDate = input.StartDate.GetValue(),
                    EndDate = input.EndDate.GetValue(),
                    Page = input.Page.GetValue(),
                    Offset = input.Page.GetValue()
                });

            return Result<Operation[]>.Error(
                message: "Não foi possível consultar as operações realizadas por um usuário em um ativo financeiro, pois um erro inesperado ocorreu.");
        }
    }
}
