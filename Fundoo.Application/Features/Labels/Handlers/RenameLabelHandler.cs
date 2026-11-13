using Fundoo.Application.Features.Labels.Commands;
using Fundoo.Application.Features.Labels.Results;
using Fundoo.Application.Features.Labels.Services;

namespace Fundoo.Application.Features.Labels.Handlers;

public sealed class RenameLabelHandler(ILabelService labelService) : IRenameLabelHandler
{
    public Task<RenameLabelResult> HandleAsync(
        RenameLabelCommand command,
        CancellationToken cancellationToken)
    {
        return labelService.RenameAsync(command, cancellationToken);
    }
}
