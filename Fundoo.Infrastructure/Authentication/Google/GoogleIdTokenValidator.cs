using Fundoo.Application.Features.Authentication.Services;
using Fundoo.Application.Features.Authentication.Models;
using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fundoo.Infrastructure.Authentication.Google;

public sealed class GoogleIdTokenValidator(
    IOptions<GoogleOptions> options,
    ILogger<GoogleIdTokenValidator> logger) : IGoogleIdTokenValidator
{
    public async Task<GoogleIdentity?> ValidateGoogleIdTokenAsync(
        string idToken,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var clientId = options.Value.ClientId;
        if (string.IsNullOrWhiteSpace(clientId))
        {
            throw new InvalidOperationException(
                "Google OAuth client ID is not configured. Set Authentication:Google:ClientId.");
        }

        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(
                idToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = [clientId]
                });

            if (string.IsNullOrWhiteSpace(payload.Subject) ||
                string.IsNullOrWhiteSpace(payload.Email) ||
                payload.EmailVerified != true)
            {
                return null;
            }

            return new GoogleIdentity(
                payload.Subject,
                payload.Email,
                payload.GivenName,
                payload.FamilyName);
        }
        catch (InvalidJwtException)
        {
            logger.LogInformation("Google sign-in was rejected because its ID token was invalid.");
            return null;
        }
    }
}
