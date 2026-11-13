using Fundoo.Application.Features.Notes.Queries;
using Fundoo.Application.Features.Notes.Results;

namespace Fundoo.Application.Features.Notes.Handlers;

public interface IGetNoteByIdHandler
{
    Task<NoteListItemResult?> HandleAsync(
        GetNoteByIdQuery query,
        CancellationToken cancellationToken);
}
