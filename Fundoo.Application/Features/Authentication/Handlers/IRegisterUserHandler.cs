using Fundoo.Application.Features.Authentication.Commands;
using Fundoo.Application.Features.Authentication.Results;

namespace Fundoo.Application.Features.Authentication.Handlers;

public interface IRegisterUserHandler
{
    Task<RegisterUserResult> HandleAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken);
}
