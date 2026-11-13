using Fundoo.Application.Features.Labels.Commands;
using Fundoo.Application.Features.Labels.Results;
using Fundoo.Application.Features.Labels.Services;

namespace Fundoo.Application.Features.Labels.Handlers;

public sealed class CreateLabelHandler(ILabelService labelService) : ICreateLabelHandler
{
    public Task<CreateLabelResult> HandleAsync(
        CreateLabelCommand command,
        CancellationToken cancellationToken)
    {
        return labelService.CreateAsync(command, cancellationToken);
    }
}
