using Fundoo.Application.Features.Authentication.Commands;
using Fundoo.Application.Features.Authentication.Services;
using Fundoo.Application.Features.Authentication.Results;
using Fundoo.Infrastructure.Email.Templates;
using Fundoo.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Fundoo.Infrastructure.Services.Authentication;

public sealed class UserAccountService(
    UserManager<FundooUser> userManager,
    SignInManager<FundooUser> signInManager,
    IFundooAccessTokenIssuer accessTokenIssuer,
    IEmailSender emailSender,
    ILogger<UserAccountService> logger)
    : IUserAccountService
{
    public async Task<UserAccountOperationResult> RegisterAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = new FundooUser
        {
            UserName = command.Email,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            FirstName = command.FirstName,
            LastName = command.LastName
        };

        var identityResult = await userManager.CreateAsync(user, command.Password);
        var errors = identityResult.Errors
            .Select(error => error.Description)
            .ToArray();

        return new UserAccountOperationResult(
            identityResult.Succeeded ? user.Id : null,
            errors);
    }

    public async Task<UserLoginCheckResult> SignInWithPasswordAsync(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await userManager.FindByEmailAsync(command.Email);

        if (user is null)
        {
            return new UserLoginCheckResult(Succeeded: false, UserId: null);
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(
            user,
            command.Password,
            lockoutOnFailure: true);

        if (!signInResult.Succeeded)
        {
            return new UserLoginCheckResult(Succeeded: false, UserId: null);
        }

        var accessToken = accessTokenIssuer.CreateAccessToken(
            user.Id,
            user.Email ?? command.Email);

        return new UserLoginCheckResult(
            Succeeded: true,
            UserId: user.Id,
            AccessToken: accessToken);
    }

    public async Task RequestPasswordResetAsync(
        ForgotPasswordCommand command,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await userManager.FindByEmailAsync(command.Email);

        if (user is null || string.IsNullOrWhiteSpace(user.Email))
        {
            return;
        }

        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
        var emailBody = PasswordResetEmailTemplate.Build(user.Email, resetToken);

        try
        {
            await emailSender.SendAsync(
                user.Email,
                "Reset your Fundoo password",
                emailBody,
                cancellationToken);
        }
        catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
        {
            logger.LogError(exception, "Failed to send a password reset email.");
        }
    }

    public async Task<PasswordResetOperationResult> ResetPasswordAsync(
        ResetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await userManager.FindByEmailAsync(command.Email);

        if (user is null)
        {
            return new PasswordResetOperationResult(
                Succeeded: false,
                Errors: ["The reset request is invalid or has expired."]);
        }

        var identityResult = await userManager.ResetPasswordAsync(
            user,
            command.Token,
            command.NewPassword);

        var errors = identityResult.Errors
            .Select(error => error.Description)
            .ToArray();

        return new PasswordResetOperationResult(
            identityResult.Succeeded,
            errors);
    }
}
