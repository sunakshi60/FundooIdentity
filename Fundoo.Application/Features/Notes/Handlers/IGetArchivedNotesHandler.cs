using Fundoo.Application.Features.Notes.Queries;
using Fundoo.Application.Features.Notes.Results;

namespace Fundoo.Application.Features.Notes.Handlers;

public interface IGetArchivedNotesHandler
{
    Task<IReadOnlyList<NoteListItemResult>> HandleAsync(
        GetArchivedNotesQuery query,
        CancellationToken cancellationToken);
}
