namespace Fundoo.Infrastructure.Authentication.Google;

public sealed class GoogleOptions
{
    public const string SectionName = "Authentication:Google";

    public string ClientId { get; set; } = string.Empty;
}
