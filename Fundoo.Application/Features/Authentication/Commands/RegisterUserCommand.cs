namespace Fundoo.Application.Features.Authentication.Commands;

public sealed record RegisterUserCommand(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email,
    string Password);
