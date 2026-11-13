using Fundoo.Application.Features.Labels.Commands;

namespace Fundoo.Application.Features.Labels.Handlers;

public interface IDeleteLabelHandler
{
    Task<bool> HandleAsync(
        DeleteLabelCommand command,
        CancellationToken cancellationToken);
}
