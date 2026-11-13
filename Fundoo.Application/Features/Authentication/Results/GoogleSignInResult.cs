using Fundoo.Application.Features.Authentication.Models;

namespace Fundoo.Application.Features.Authentication.Results;

public sealed record GoogleSignInResult(
    bool Succeeded,
    string? UserId,
    string? Email,
    FundooAccessToken? AccessToken)
{
    public static GoogleSignInResult Failure() =>
        new(false, null, null, null);
}
