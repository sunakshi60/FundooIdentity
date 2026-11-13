namespace Fundoo.Infrastructure.Authentication.Tokens;

public sealed class FundooTokenOptions
{
    public const string SectionName = "Authentication:FundooToken";

    public string Issuer { get; set; } = "Fundoo.Api";

    public string Audience { get; set; } = "Fundoo.Api";

    public string SigningKey { get; set; } = string.Empty;

    public int LifetimeMinutes { get; set; } = 30;
}
