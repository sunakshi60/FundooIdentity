using Fundoo.Application.Features.Notes.Commands;
using Fundoo.Application.Features.Notes.Services;

namespace Fundoo.Application.Features.Notes.Handlers;

public sealed class SetNoteLabelAssociationHandler(INoteService noteService)
    : ISetNoteLabelAssociationHandler
{
    public Task<bool> HandleAsync(
        SetNoteLabelAssociationCommand command,
        CancellationToken cancellationToken)
    {
        return noteService.SetLabelAssociationAsync(command, cancellationToken);
    }
}
