using Microsoft.AspNetCore.Identity;

namespace Fundoo.Infrastructure.Identity;

public sealed class FundooUser : IdentityUser
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}
