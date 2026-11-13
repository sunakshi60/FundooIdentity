using Fundoo.Application.Features.Authentication.Commands;

namespace Fundoo.Application.Features.Authentication.Handlers;

public interface IForgotPasswordHandler
{
    Task HandleAsync(
        ForgotPasswordCommand command,
        CancellationToken cancellationToken);
}
