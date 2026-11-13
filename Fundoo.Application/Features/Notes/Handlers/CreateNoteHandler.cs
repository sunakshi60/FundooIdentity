using Fundoo.Application.Features.Notes.Commands;
using Fundoo.Application.Features.Notes.Results;
using Fundoo.Application.Features.Notes.Services;

namespace Fundoo.Application.Features.Notes.Handlers;

public sealed class CreateNoteHandler(INoteService noteService) : ICreateNoteHandler
{
    public Task<CreateNoteResult> HandleAsync(
        CreateNoteCommand command,
        CancellationToken cancellationToken)
    {
        return noteService.CreateAsync(command, cancellationToken);
    }
}
