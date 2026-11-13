using Microsoft.AspNetCore.Mvc;
using Fundoo.Application.Features.Authentication.Commands;
using Fundoo.Application.Features.Authentication.Handlers;
using Fundoo.Api.Requests.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace Fundoo.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IRegisterUserHandler _registerUserHandler;
    private readonly ILoginUserHandler _loginUserHandler;
    private readonly IForgotPasswordHandler _forgotPasswordHandler;
    private readonly IResetPasswordHandler _resetPasswordHandler;
    private readonly IGoogleSignInHandler _googleSignInHandler;

    public AuthenticationController(
        IRegisterUserHandler registerUserHandler,
        ILoginUserHandler loginUserHandler,
        IForgotPasswordHandler forgotPasswordHandler,
        IResetPasswordHandler resetPasswordHandler,
        IGoogleSignInHandler googleSignInHandler)
    {
        _registerUserHandler = registerUserHandler;
        _loginUserHandler = loginUserHandler;
        _forgotPasswordHandler = forgotPasswordHandler;
        _resetPasswordHandler = resetPasswordHandler;
        _googleSignInHandler = googleSignInHandler;
    }

    [AllowAnonymous]
    [HttpPost("google")]
    public async Task<IActionResult> SignInWithGoogle(
        [FromBody] GoogleSignInRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _googleSignInHandler.HandleAsync(
            new GoogleSignInCommand(request.IdToken),
            cancellationToken);

        if (!result.Succeeded)
        {
            return Unauthorized(new { message = "Google sign-in could not be verified." });
        }

        return Ok(new
        {
            userId = result.UserId,
            email = result.Email,
            tokenType = "Bearer",
            accessToken = result.AccessToken?.Value,
            expiresAt = result.AccessToken?.ExpiresAt
        });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
            email = User.FindFirst(JwtRegisteredClaimNames.Email)?.Value
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.Email,
            request.Password);

        var result = await _registerUserHandler.HandleAsync(command, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors });
        }

        return StatusCode(StatusCodes.Status201Created, new { userId = result.UserId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> SignInWithPassword(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await _loginUserHandler.HandleAsync(command, cancellationToken);

        if (!result.Succeeded)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        return Ok(new
        {
            userId = result.UserId,
            tokenType = "Bearer",
            accessToken = result.AccessToken,
            expiresAt = result.ExpiresAt
        });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> RequestPasswordReset(
        [FromBody] ForgotPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ForgotPasswordCommand(request.Email);
        await _forgotPasswordHandler.HandleAsync(command, cancellationToken);

        return Ok(new
        {
            message = "If an account exists for that email, a password reset message has been sent."
        });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ResetPasswordCommand(
            request.Email,
            request.Token,
            request.NewPassword);

        var result = await _resetPasswordHandler.HandleAsync(command, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors });
        }

        return Ok(new { message = "Password has been reset successfully." });
    }
}
