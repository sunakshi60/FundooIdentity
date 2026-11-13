using Fundoo.Application.Features.Notes.Commands;

namespace Fundoo.Application.Features.Notes.Handlers;

public interface IRestoreNoteHandler
{
    Task<bool> HandleAsync(
        RestoreNoteCommand command,
        CancellationToken cancellationToken);
}
