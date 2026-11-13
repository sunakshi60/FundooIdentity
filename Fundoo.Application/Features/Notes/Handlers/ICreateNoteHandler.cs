using Fundoo.Application.Features.Notes.Commands;
using Fundoo.Application.Features.Notes.Results;

namespace Fundoo.Application.Features.Notes.Handlers;

public interface ICreateNoteHandler
{
    Task<CreateNoteResult> HandleAsync(
        CreateNoteCommand command,
        CancellationToken cancellationToken);
}
