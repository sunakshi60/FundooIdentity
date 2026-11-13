namespace Fundoo.Application.Features.Notes.Commands;

public sealed record RestoreNoteCommand(Guid NoteId, string UserId);
