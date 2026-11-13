namespace Fundoo.Api.Requests.Authentication;

public sealed record ResetPasswordRequest(
    string Email,
    string Token,
    string NewPassword);
