using Fundoo.Application.Features.Authentication.Commands;
using Fundoo.Application.Features.Authentication.Services;
using Fundoo.Application.Features.Authentication.Results;

namespace Fundoo.Application.Features.Authentication.Handlers;

public sealed class RegisterUserHandler(
    IUserAccountService userAccountService)
    : IRegisterUserHandler
{
    public async Task<RegisterUserResult> HandleAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await userAccountService.RegisterAsync(
            command,
            cancellationToken);

        return new RegisterUserResult(result.UserId, result.Errors);
    }
}
