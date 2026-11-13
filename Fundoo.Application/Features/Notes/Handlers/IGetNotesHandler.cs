using Fundoo.Application.Features.Notes.Queries;
using Fundoo.Application.Features.Notes.Results;

namespace Fundoo.Application.Features.Notes.Handlers;

public interface IGetNotesHandler
{
    Task<IReadOnlyList<NoteListItemResult>> HandleAsync(
        GetNotesQuery query,
        CancellationToken cancellationToken);
}
