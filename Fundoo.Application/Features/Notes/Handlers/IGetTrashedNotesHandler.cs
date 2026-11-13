using Fundoo.Application.Features.Notes.Queries;
using Fundoo.Application.Features.Notes.Results;

namespace Fundoo.Application.Features.Notes.Handlers;

public interface IGetTrashedNotesHandler
{
    Task<IReadOnlyList<NoteListItemResult>> HandleAsync(
        GetTrashedNotesQuery query,
        CancellationToken cancellationToken);
}
