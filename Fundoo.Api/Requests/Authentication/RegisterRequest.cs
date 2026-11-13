namespace Fundoo.Api.Requests.Authentication;

public sealed record RegisterRequest(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email,
    string Password);
