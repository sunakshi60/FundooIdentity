namespace Fundoo.Application.Features.Authentication.Results;

public sealed record PasswordResetOperationResult(
    bool Succeeded,
    IReadOnlyCollection<string> Errors);
