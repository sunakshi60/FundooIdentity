using Fundoo.Application.Features.Authentication.Services;
using Fundoo.Application.Features.Authentication.Models;
using Fundoo.Application.Features.Authentication.Results;
using Fundoo.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Fundoo.Infrastructure.Services.Authentication;

public sealed class GoogleSignInService(
    IGoogleIdTokenValidator googleIdTokenValidator,
    UserManager<FundooUser> userManager,
    IFundooAccessTokenIssuer accessTokenIssuer,
    ILogger<GoogleSignInService> logger) : IGoogleSignInService
{
    private const string GoogleLoginProvider = "Google";

    public async Task<GoogleSignInResult> SignInWithGoogleAsync(
        string idToken,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var googleIdentity = await googleIdTokenValidator.ValidateGoogleIdTokenAsync(
            idToken,
            cancellationToken);

        if (googleIdentity is null)
        {
            return GoogleSignInResult.Failure();
        }

        var user = await userManager.FindByLoginAsync(
            GoogleLoginProvider,
            googleIdentity.Subject);

        if (user is null)
        {
            user = await userManager.FindByEmailAsync(googleIdentity.Email);

            if (user is null)
            {
                user = new FundooUser
                {
                    UserName = googleIdentity.Email,
                    Email = googleIdentity.Email,
                    EmailConfirmed = true,
                    FirstName = string.IsNullOrWhiteSpace(googleIdentity.FirstName)
                        ? "Fundoo"
                        : googleIdentity.FirstName,
                    LastName = string.IsNullOrWhiteSpace(googleIdentity.LastName)
                        ? "User"
                        : googleIdentity.LastName
                };

                var createResult = await userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    logger.LogWarning(
                        "Could not create a Fundoo account for a verified Google identity.");
                    return GoogleSignInResult.Failure();
                }
            }
            else if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return GoogleSignInResult.Failure();
                }
            }

            var addLoginResult = await userManager.AddLoginAsync(
                user,
                new UserLoginInfo(
                    GoogleLoginProvider,
                    googleIdentity.Subject,
                    GoogleLoginProvider));

            if (!addLoginResult.Succeeded)
            {
                logger.LogWarning(
                    "Could not link a verified Google identity to the Fundoo account.");
                return GoogleSignInResult.Failure();
            }
        }

        var accessToken = accessTokenIssuer.CreateAccessToken(
            user.Id,
            user.Email ?? googleIdentity.Email);

        return new GoogleSignInResult(
            true,
            user.Id,
            user.Email ?? googleIdentity.Email,
            accessToken);
    }
}
