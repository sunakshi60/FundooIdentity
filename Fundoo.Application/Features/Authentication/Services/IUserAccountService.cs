using Fundoo.Application.Features.Authentication.Commands;
using Fundoo.Application.Features.Authentication.Results;

namespace Fundoo.Application.Features.Authentication.Services;

public interface IUserAccountService
{
    Task<UserAccountOperationResult> RegisterAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken);

    Task<UserLoginCheckResult> SignInWithPasswordAsync(
        LoginCommand command,
        CancellationToken cancellationToken);

    Task RequestPasswordResetAsync(
        ForgotPasswordCommand command,
        CancellationToken cancellationToken);

    Task<PasswordResetOperationResult> ResetPasswordAsync(
        ResetPasswordCommand command,
        CancellationToken cancellationToken);
}
