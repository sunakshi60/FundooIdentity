using Fundoo.Application.Features.Notes.Queries;
using Fundoo.Application.Features.Notes.Results;
using Fundoo.Application.Features.Notes.Services;

namespace Fundoo.Application.Features.Notes.Handlers;

public sealed class GetTrashedNotesHandler(INoteService noteService)
    : IGetTrashedNotesHandler
{
    public Task<IReadOnlyList<NoteListItemResult>> HandleAsync(
        GetTrashedNotesQuery query,
        CancellationToken cancellationToken)
    {
        return noteService.GetTrashedByUserAsync(query, cancellationToken);
    }
}
