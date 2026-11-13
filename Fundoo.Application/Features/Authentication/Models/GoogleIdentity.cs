namespace Fundoo.Application.Features.Authentication.Models;

public sealed record GoogleIdentity(
    string Subject,
    string Email,
    string? FirstName,
    string? LastName);
