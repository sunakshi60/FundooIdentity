namespace Fundoo.Application.Features.Authentication.Commands;

public sealed record ResetPasswordCommand(
    string Email,
    string Token,
    string NewPassword);
