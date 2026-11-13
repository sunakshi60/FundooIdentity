using Fundoo.Application.Features.Labels.Commands;
using Fundoo.Application.Features.Labels.Results;

namespace Fundoo.Application.Features.Labels.Handlers;

public interface IRenameLabelHandler
{
    Task<RenameLabelResult> HandleAsync(
        RenameLabelCommand command,
        CancellationToken cancellationToken);
}
