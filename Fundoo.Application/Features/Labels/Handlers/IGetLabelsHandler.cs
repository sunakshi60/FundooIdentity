using Fundoo.Application.Features.Labels.Queries;
using Fundoo.Application.Features.Labels.Results;

namespace Fundoo.Application.Features.Labels.Handlers;

public interface IGetLabelsHandler
{
    Task<IReadOnlyList<LabelResult>> HandleAsync(
        GetLabelsQuery query,
        CancellationToken cancellationToken);
}
