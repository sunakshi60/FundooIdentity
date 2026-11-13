using Fundoo.Application.Features.Notes.Commands;
using Fundoo.Application.Features.Notes.Services;

namespace Fundoo.Application.Features.Notes.Handlers;

public sealed class RestoreNoteHandler(INoteService noteService) : IRestoreNoteHandler
{
    public Task<bool> HandleAsync(
        RestoreNoteCommand command,
        CancellationToken cancellationToken)
    {
        return noteService.RestoreAsync(command, cancellationToken);
    }
}
