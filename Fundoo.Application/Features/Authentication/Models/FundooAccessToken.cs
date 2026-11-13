namespace Fundoo.Application.Features.Authentication.Models;

public sealed record FundooAccessToken(
    string Value,
    DateTimeOffset ExpiresAt);
