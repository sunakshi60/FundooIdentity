using Fundoo.Application.Features.Authentication.Commands;
using Fundoo.Application.Features.Authentication.Services;
using Fundoo.Application.Features.Authentication.Results;

namespace Fundoo.Application.Features.Authentication.Handlers;

public sealed class LoginUserHandler(
    IUserAccountService userAccountService)
    : ILoginUserHandler
{
    public async Task<LoginUserResult> HandleAsync(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var result = await userAccountService.SignInWithPasswordAsync(
            command,
            cancellationToken);

        return new LoginUserResult(
            result.Succeeded,
            result.UserId,
            result.AccessToken?.Value,
            result.AccessToken?.ExpiresAt);
    }
}
