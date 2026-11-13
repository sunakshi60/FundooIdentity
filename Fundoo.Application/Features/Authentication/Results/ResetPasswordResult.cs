namespace Fundoo.Application.Features.Authentication.Results;

public sealed record ResetPasswordResult(
    bool Succeeded,
    IReadOnlyCollection<string> Errors);
