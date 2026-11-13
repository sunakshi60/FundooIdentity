using Fundoo.Application.Features.Authentication.Results;

namespace Fundoo.Application.Features.Authentication.Services;

public interface IGoogleSignInService
{
    Task<GoogleSignInResult> SignInWithGoogleAsync(
        string idToken,
        CancellationToken cancellationToken);
}
