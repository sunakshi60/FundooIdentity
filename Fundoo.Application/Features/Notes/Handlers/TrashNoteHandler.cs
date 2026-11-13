using Fundoo.Application.Features.Notes.Commands;
using Fundoo.Application.Features.Notes.Services;

namespace Fundoo.Application.Features.Notes.Handlers;

public sealed class TrashNoteHandler(INoteService noteService) : ITrashNoteHandler
{
    public Task<bool> HandleAsync(
        TrashNoteCommand command,
        CancellationToken cancellationToken)
    {
        return noteService.TrashAsync(command, cancellationToken);
    }
}
