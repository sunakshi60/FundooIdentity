using Fundoo.Application.Features.Notes.Commands;
using Fundoo.Application.Features.Notes.Services;

namespace Fundoo.Application.Features.Notes.Handlers;

public sealed class SetNoteArchiveStatusHandler(INoteService noteService)
    : ISetNoteArchiveStatusHandler
{
    public Task<bool> HandleAsync(
        SetNoteArchiveStatusCommand command,
        CancellationToken cancellationToken)
    {
        return noteService.SetArchivedAsync(command, cancellationToken);
    }
}
