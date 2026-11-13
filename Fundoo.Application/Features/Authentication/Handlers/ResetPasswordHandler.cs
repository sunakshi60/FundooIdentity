using Fundoo.Application.Features.Authentication.Commands;
using Fundoo.Application.Features.Authentication.Services;
using Fundoo.Application.Features.Authentication.Results;

namespace Fundoo.Application.Features.Authentication.Handlers;

public sealed class ResetPasswordHandler(
    IUserAccountService userAccountService)
    : IResetPasswordHandler
{
    public async Task<ResetPasswordResult> HandleAsync(
        ResetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var result = await userAccountService.ResetPasswordAsync(
            command,
            cancellationToken);

        return new ResetPasswordResult(result.Succeeded, result.Errors);
    }
}
