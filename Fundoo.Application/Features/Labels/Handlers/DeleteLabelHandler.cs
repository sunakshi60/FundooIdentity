using Fundoo.Application.Features.Labels.Commands;
using Fundoo.Application.Features.Labels.Services;

namespace Fundoo.Application.Features.Labels.Handlers;

public sealed class DeleteLabelHandler(ILabelService labelService) : IDeleteLabelHandler
{
    public Task<bool> HandleAsync(
        DeleteLabelCommand command,
        CancellationToken cancellationToken)
    {
        return labelService.DeleteAsync(command, cancellationToken);
    }
}
