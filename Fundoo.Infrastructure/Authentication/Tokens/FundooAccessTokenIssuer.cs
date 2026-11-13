using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fundoo.Application.Features.Authentication.Services;
using Fundoo.Application.Features.Authentication.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Fundoo.Infrastructure.Authentication.Tokens;

public sealed class FundooAccessTokenIssuer(
    IOptions<FundooTokenOptions> options) : IFundooAccessTokenIssuer
{
    public FundooAccessToken CreateAccessToken(string userId, string email)
    {
        var settings = options.Value;
        if (string.IsNullOrWhiteSpace(settings.SigningKey) ||
            Encoding.UTF8.GetByteCount(settings.SigningKey) < 32)
        {
            throw new InvalidOperationException(
                "Authentication:FundooToken:SigningKey must contain at least 32 UTF-8 bytes. Store it in User Secrets.");
        }

        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.AddMinutes(settings.LifetimeMinutes);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(settings.SigningKey));
        var credentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        return new FundooAccessToken(
            new JwtSecurityTokenHandler().WriteToken(token),
            expiresAt);
    }
}
