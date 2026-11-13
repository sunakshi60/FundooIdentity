using Fundoo.Application.Features.Notes.Queries;
using Fundoo.Application.Features.Notes.Results;
using Fundoo.Application.Features.Notes.Services;

namespace Fundoo.Application.Features.Notes.Handlers;

public sealed class GetNotesHandler(INoteService noteService) : IGetNotesHandler
{
    public Task<IReadOnlyList<NoteListItemResult>> HandleAsync(
        GetNotesQuery query,
        CancellationToken cancellationToken)
    {
        return noteService.GetByUserAsync(query, cancellationToken);
    }
}
