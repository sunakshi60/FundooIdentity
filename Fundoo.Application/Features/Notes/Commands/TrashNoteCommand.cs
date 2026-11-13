namespace Fundoo.Application.Features.Notes.Commands;

public sealed record TrashNoteCommand(Guid NoteId, string UserId);
