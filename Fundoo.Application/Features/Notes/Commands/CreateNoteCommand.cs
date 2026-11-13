namespace Fundoo.Application.Features.Notes.Commands;

public sealed record CreateNoteCommand(
    string UserId,
    string Title,
    string? Content);
