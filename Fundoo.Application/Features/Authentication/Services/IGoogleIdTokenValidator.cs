using Fundoo.Application.Features.Authentication.Models;

namespace Fundoo.Application.Features.Authentication.Services;

public interface IGoogleIdTokenValidator
{
    Task<GoogleIdentity?> ValidateGoogleIdTokenAsync(
        string idToken,
        CancellationToken cancellationToken);
}
