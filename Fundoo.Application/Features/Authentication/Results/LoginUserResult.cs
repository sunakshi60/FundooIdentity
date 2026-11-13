namespace Fundoo.Application.Features.Authentication.Results;

public sealed record LoginUserResult(
    bool Succeeded,
    string? UserId,
    string? AccessToken,
    DateTimeOffset? ExpiresAt);
