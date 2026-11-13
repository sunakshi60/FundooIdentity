using Fundoo.Application.Features.Notes.Queries;
using Fundoo.Application.Features.Notes.Results;
using Fundoo.Application.Features.Notes.Services;

namespace Fundoo.Application.Features.Notes.Handlers;

public sealed class GetArchivedNotesHandler(INoteService noteService)
    : IGetArchivedNotesHandler
{
    public Task<IReadOnlyList<NoteListItemResult>> HandleAsync(
        GetArchivedNotesQuery query,
        CancellationToken cancellationToken)
    {
        return noteService.GetArchivedByUserAsync(query, cancellationToken);
    }
}
