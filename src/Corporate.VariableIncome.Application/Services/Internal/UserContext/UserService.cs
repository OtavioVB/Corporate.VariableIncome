using Corporate.VariableIncome.Application.Services.Internal.UserContext.Interfaces;
using Corporate.VariableIncome.Domain.BoundedContexts.UserContext.Entities;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Domain.ValueObjects;
using Corporate.VariableIncome.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using Microsoft.Extensions.Logging;

namespace Corporate.VariableIncome.Application.Services.Internal.UserContext;

public sealed class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IExtensionUserRepository _extensionUserRepository;

    public UserService(ILogger<UserService> logger, IExtensionUserRepository extensionUserRepository)
    {
        _logger = logger;
        _extensionUserRepository = extensionUserRepository;
    }

    public async Task<Result<User>> GetCreateOperationUserEligibilityAsync(IdValueObject userId, CancellationToken cancellationToken)
    {
        try
        {
            if (!userId.IsValid)
                return Result<User>.Error(
                    message: "O ID de identificação do usuário precisa ser válido.");

            var user = await _extensionUserRepository.GetUserAsNoTrackingByIdAsync(
                userId: userId,
                cancellationToken: cancellationToken);

            if (user == null)
                return Result<User>.Error(
                    message: "Não foi possível encontrar um usuário com o ID enviado.");

            /*
             * Alguma regra de elegibilidade, como status da conta, allow list, block list, etc
             */

            return Result<User>.Success(
                value: user);
        }
        catch (Exception ex)
        {
            _logger?.LogError(
                exception: ex,
                message: "[{Type}][{Method}] Is not possible to get create operation user elegibility because unhandled exception has been throwed. Input = {@Input}.",
                nameof(UserService),
                nameof(GetCreateOperationUserEligibilityAsync),
                new
                {
                    UserId = userId
                });

            return Result<User>.Error(
                message: "Não foi possível consultar elegibilidade do usuário para criação de operação de compra/venda para ativos financeiros.");
        }
    }
}
