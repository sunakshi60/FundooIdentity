namespace Fundoo.Application.Features.Notes.Commands;

public sealed record UpdateNoteCommand(
    Guid NoteId,
    string UserId,
    string Title,
    string? Content);
