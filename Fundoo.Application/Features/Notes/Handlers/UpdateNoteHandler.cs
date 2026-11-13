using Fundoo.Application.Features.Notes.Commands;
using Fundoo.Application.Features.Notes.Results;
using Fundoo.Application.Features.Notes.Services;

namespace Fundoo.Application.Features.Notes.Handlers;

public sealed class UpdateNoteHandler(INoteService noteService) : IUpdateNoteHandler
{
    public Task<UpdateNoteResult?> HandleAsync(
        UpdateNoteCommand command,
        CancellationToken cancellationToken)
    {
        return noteService.UpdateAsync(command, cancellationToken);
    }
}
