using Fundoo.Application.Features.Authentication.Models;

namespace Fundoo.Application.Features.Authentication.Services;

public interface IFundooAccessTokenIssuer
{
    FundooAccessToken CreateAccessToken(string userId, string email);
}
