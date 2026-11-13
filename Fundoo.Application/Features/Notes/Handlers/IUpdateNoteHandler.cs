using Fundoo.Application.Features.Notes.Commands;
using Fundoo.Application.Features.Notes.Results;

namespace Fundoo.Application.Features.Notes.Handlers;

public interface IUpdateNoteHandler
{
    Task<UpdateNoteResult?> HandleAsync(
        UpdateNoteCommand command,
        CancellationToken cancellationToken);
}
