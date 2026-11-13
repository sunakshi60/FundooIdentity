using Fundoo.Application.Features.Authentication.Commands;
using Fundoo.Application.Features.Authentication.Services;

namespace Fundoo.Application.Features.Authentication.Handlers;

public sealed class ForgotPasswordHandler(
    IUserAccountService userAccountService)
    : IForgotPasswordHandler
{
    public Task HandleAsync(
        ForgotPasswordCommand command,
        CancellationToken cancellationToken)
    {
        return userAccountService.RequestPasswordResetAsync(command, cancellationToken);
    }
}
