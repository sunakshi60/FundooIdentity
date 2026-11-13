using Fundoo.Application.Features.Authentication.Commands;
using Fundoo.Application.Features.Authentication.Results;

namespace Fundoo.Application.Features.Authentication.Handlers;

public interface IResetPasswordHandler
{
    Task<ResetPasswordResult> HandleAsync(
        ResetPasswordCommand command,
        CancellationToken cancellationToken);
}
