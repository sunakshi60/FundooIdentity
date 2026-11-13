namespace Fundoo.Application.Features.Authentication.Results;

public sealed record RegisterUserResult(
    string? UserId,
    IReadOnlyCollection<string> Errors)
{
    public bool Succeeded => Errors.Count == 0;
}
