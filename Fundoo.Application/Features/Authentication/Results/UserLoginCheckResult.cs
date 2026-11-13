using Fundoo.Application.Features.Authentication.Models;

namespace Fundoo.Application.Features.Authentication.Results;

public sealed record UserLoginCheckResult(
    bool Succeeded,
    string? UserId,
    FundooAccessToken? AccessToken = null);
