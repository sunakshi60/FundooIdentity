using Fundoo.Application.Features.Notes.Commands;

namespace Fundoo.Application.Features.Notes.Handlers;

public interface ISetNoteLabelAssociationHandler
{
    Task<bool> HandleAsync(
        SetNoteLabelAssociationCommand command,
        CancellationToken cancellationToken);
}
