namespace Fundoo.Application.Features.Notes.Commands;

public sealed record SetNoteArchiveStatusCommand(
    Guid NoteId,
    string UserId,
    bool IsArchived);
