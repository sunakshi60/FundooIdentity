using Fundoo.Application.Features.Notes.Queries;
using Fundoo.Application.Features.Notes.Results;
using Fundoo.Application.Features.Notes.Services;

namespace Fundoo.Application.Features.Notes.Handlers;

public sealed class GetNoteByIdHandler(INoteService noteService) : IGetNoteByIdHandler
{
    public Task<NoteListItemResult?> HandleAsync(
        GetNoteByIdQuery query,
        CancellationToken cancellationToken)
    {
        return noteService.GetByIdForUserAsync(query, cancellationToken);
    }
}
