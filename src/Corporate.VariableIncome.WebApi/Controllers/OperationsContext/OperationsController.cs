using Corporate.VariableIncome.Application.UseCases.Interfaces;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Inputs;
using Corporate.VariableIncome.Application.UseCases.OperationContext.CreateOperation.Outputs;
using Corporate.VariableIncome.Application.UseCases.OperationContext.GetUserOperations.Inputs;
using Corporate.VariableIncome.Application.UseCases.OperationContext.GetUserOperations.Outputs;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.WebApi.Controllers.OperationsContext.Requests;
using Corporate.VariableIncome.WebApi.Controllers.OperationsContext.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Corporate.VariableIncome.WebApi.Controllers.OperationsContext;

[Route("/api/v1/operations")]
[ApiController]
public sealed class OperationsController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> HttpGetOperationsAsync(
        [FromServices] IUseCase<GetAssetUserOperationsUseCaseInput, Result<GetAssetUserOperationsUseCaseOutput>> useCase,
        [FromQuery] Guid assetId,
        [FromQuery] Guid userId,
        [FromQuery] int page,
        [FromQuery] int offset,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        CancellationToken cancellationToken)
    {
        var useCaseInput = GetAssetUserOperationsUseCaseInput.Factory(
            userId: userId,
            assetId: assetId,
            page: page,
            offset: offset,
            startDate: startDate,
            endDate: endDate);

        var useCaseResult = await useCase.ExecuteUseCaseAsync(
            input: useCaseInput,
            cancellationToken: cancellationToken);

        if (useCaseResult.IsFailed)
            return StatusCode(
                statusCode: StatusCodes.Status400BadRequest,
                value: useCaseResult);

        return StatusCode(
            statusCode: StatusCodes.Status200OK,
            value: GetUserAssetsOperationResponse.Factory(useCaseResult.GetValue().Operations));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> HttpPostCreateOperationAsync(
        [FromServices] IUseCase<CreateOperationUseCaseInput, Result<CreateOperationUseCaseOutput>> useCase,
        [FromBody] CreateOperationRequest request,
        CancellationToken cancellationToken)
    {
        var input = CreateOperationUseCaseInput.Factory(
            assetId: request.AssetId,
            userId: request.UserId,
            quantity: request.Quantity,
            typeOperation: request.Operation);

        var useCaseResult = await useCase.ExecuteUseCaseAsync(input, cancellationToken);

        if (useCaseResult.IsFailed)
            return StatusCode(
                statusCode: StatusCodes.Status400BadRequest,
                value: useCaseResult);

        var output = CreateOperationResponse.Build(
            position: useCaseResult.GetValue().Position,
            operation: useCaseResult.GetValue().Operation);

        return StatusCode(
            statusCode: StatusCodes.Status201Created,
            value: output);
    }
}
