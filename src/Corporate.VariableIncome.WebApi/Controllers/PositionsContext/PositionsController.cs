using Corporate.VariableIncome.Application.UseCases.Interfaces;
using Corporate.VariableIncome.Application.UseCases.PositionContext.GetActualPosition.Inputs;
using Corporate.VariableIncome.Domain.BoundedContexts.PositionContext.Entities;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.WebApi.Controllers.PositionsContext.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Corporate.VariableIncome.WebApi.Controllers.PositionsContext;

[Route("/api/v1/positions")]
[ApiController]
public sealed class PositionsController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> HttpGetActualPositionAsync(
        [FromServices] IUseCase<GetUserActualPositionUseCaseInput, Result<PositionSnapshot>> useCase,
        [FromQuery] string assetCode,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var useCaseInput = GetUserActualPositionUseCaseInput.Factory(
            code: assetCode,
            userId: userId);

        var useCaseResult = await useCase.ExecuteUseCaseAsync(useCaseInput, cancellationToken);

        if (useCaseResult.IsFailed)
            return StatusCode(
                statusCode: StatusCodes.Status400BadRequest,
                value: useCaseResult);

        return StatusCode(
            statusCode: StatusCodes.Status200OK,
            value: GetUserActualPositionResponse.Factory(useCaseResult.GetValue()));
    }
}
