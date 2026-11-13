using Fundoo.Application.Features.Labels.Commands;
using Fundoo.Application.Features.Labels.Results;

namespace Fundoo.Application.Features.Labels.Handlers;

public interface ICreateLabelHandler
{
    Task<CreateLabelResult> HandleAsync(
        CreateLabelCommand command,
        CancellationToken cancellationToken);
}
