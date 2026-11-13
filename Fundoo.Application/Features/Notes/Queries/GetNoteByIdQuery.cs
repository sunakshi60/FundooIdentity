namespace Fundoo.Application.Features.Notes.Queries;

public sealed record GetNoteByIdQuery(Guid NoteId, string UserId);
