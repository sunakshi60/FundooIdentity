using Fundoo.Application.Features.Notes.Commands;

namespace Fundoo.Application.Features.Notes.Handlers;

public interface ITrashNoteHandler
{
    Task<bool> HandleAsync(
        TrashNoteCommand command,
        CancellationToken cancellationToken);
}
