using Fundoo.Application.Features.Labels.Queries;
using Fundoo.Application.Features.Labels.Results;
using Fundoo.Application.Features.Labels.Services;

namespace Fundoo.Application.Features.Labels.Handlers;

public sealed class GetLabelsHandler(ILabelService labelService) : IGetLabelsHandler
{
    public Task<IReadOnlyList<LabelResult>> HandleAsync(
        GetLabelsQuery query,
        CancellationToken cancellationToken)
    {
        return labelService.GetByUserAsync(query, cancellationToken);
    }
}
