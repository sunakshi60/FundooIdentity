using Fundoo.Application.Features.Authentication.Commands;
using Fundoo.Application.Features.Authentication.Results;

namespace Fundoo.Application.Features.Authentication.Handlers;

public interface ILoginUserHandler
{
    Task<LoginUserResult> HandleAsync(
        LoginCommand command,
        CancellationToken cancellationToken);
}
