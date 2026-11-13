namespace Fundoo.Application.Features.Authentication.Commands;

public sealed record LoginCommand(
    string Email,
    string Password);
