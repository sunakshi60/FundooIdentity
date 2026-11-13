using Fundoo.Application.Features.Authentication.Commands;
using Fundoo.Application.Features.Authentication.Services;
using Fundoo.Application.Features.Authentication.Results;

namespace Fundoo.Application.Features.Authentication.Handlers;

public sealed class GoogleSignInHandler(IGoogleSignInService googleSignInService)
    : IGoogleSignInHandler
{
    public Task<GoogleSignInResult> HandleAsync(
        GoogleSignInCommand command,
        CancellationToken cancellationToken)
    {
        return googleSignInService.SignInWithGoogleAsync(
            command.IdToken,
            cancellationToken);
    }
}
