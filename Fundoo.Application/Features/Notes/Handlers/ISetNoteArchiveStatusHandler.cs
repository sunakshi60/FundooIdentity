using Fundoo.Application.Features.Notes.Commands;

namespace Fundoo.Application.Features.Notes.Handlers;

public interface ISetNoteArchiveStatusHandler
{
    Task<bool> HandleAsync(
        SetNoteArchiveStatusCommand command,
        CancellationToken cancellationToken);
}
