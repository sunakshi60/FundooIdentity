using Fundoo.Application.Features.Labels.Commands;
using Fundoo.Application.Features.Labels.Queries;
using Fundoo.Application.Features.Labels.Results;

namespace Fundoo.Application.Features.Labels.Services;

public interface ILabelService
{
    Task<CreateLabelResult> CreateAsync(
        CreateLabelCommand command,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<LabelResult>> GetByUserAsync(
        GetLabelsQuery query,
        CancellationToken cancellationToken);

    Task<RenameLabelResult> RenameAsync(
        RenameLabelCommand command,
        CancellationToken cancellationToken);

    Task<bool> DeleteAsync(
        DeleteLabelCommand command,
        CancellationToken cancellationToken);
}
