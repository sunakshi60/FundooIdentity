using Fundoo.Application.Features.Authentication.Commands;
using Fundoo.Application.Features.Authentication.Results;

namespace Fundoo.Application.Features.Authentication.Handlers;

public interface IGoogleSignInHandler
{
    Task<GoogleSignInResult> HandleAsync(
        GoogleSignInCommand command,
        CancellationToken cancellationToken);
}
