using Fundoo.Application.Features.Labels.Commands;
using Fundoo.Application.Features.Labels.Queries;
using Fundoo.Application.Features.Labels.Repositories;
using Fundoo.Application.Features.Labels.Results;
using Fundoo.Application.Features.Labels.Services;
using Fundoo.Domain.Entities;

namespace Fundoo.Infrastructure.Services.Labels;

public sealed class LabelService(
    ILabelRepository labelRepository,
    TimeProvider timeProvider) : ILabelService
{
    public async Task<CreateLabelResult> CreateAsync(
        CreateLabelCommand command,
        CancellationToken cancellationToken)
    {
        var name = command.Name.Trim();
        var normalizedName = name.ToUpperInvariant();

        var alreadyExists = await labelRepository.ExistsByNormalizedNameAsync(
            command.UserId,
            normalizedName,
            cancellationToken);

        if (alreadyExists)
        {
            return new CreateLabelResult(
                Succeeded: false,
                Label: null,
                ErrorMessage: "A label with this name already exists.");
        }

        var label = new Label
        {
            UserId = command.UserId,
            Name = name,
            NormalizedName = normalizedName,
            CreatedAtUtc = timeProvider.GetUtcNow()
        };

        var added = await labelRepository.AddAsync(label, cancellationToken);

        if (!added)
        {
            return new CreateLabelResult(
                Succeeded: false,
                Label: null,
                ErrorMessage: "A label with this name already exists.");
        }

        return new CreateLabelResult(
            Succeeded: true,
            Label: new LabelResult(label.Id, label.Name, label.CreatedAtUtc),
            ErrorMessage: null);
    }

    public async Task<IReadOnlyList<LabelResult>> GetByUserAsync(
        GetLabelsQuery query,
        CancellationToken cancellationToken)
    {
        var labels = await labelRepository.GetByUserIdAsync(
            query.UserId,
            cancellationToken);

        return labels
            .Select(label => new LabelResult(
                label.Id,
                label.Name,
                label.CreatedAtUtc))
            .ToArray();
    }

    public async Task<RenameLabelResult> RenameAsync(
        RenameLabelCommand command,
        CancellationToken cancellationToken)
    {
        var label = await labelRepository.GetForUpdateByIdAsync(
            command.LabelId,
            command.UserId,
            cancellationToken);

        if (label is null)
        {
            return new RenameLabelResult(RenameLabelStatus.NotFound);
        }

        var name = command.Name.Trim();
        var normalizedName = name.ToUpperInvariant();

        if (label.NormalizedName != normalizedName)
        {
            var alreadyExists = await labelRepository.ExistsByNormalizedNameAsync(
                command.UserId,
                normalizedName,
                cancellationToken);

            if (alreadyExists)
            {
                return new RenameLabelResult(RenameLabelStatus.DuplicateName);
            }
        }

        label.Name = name;
        label.NormalizedName = normalizedName;

        if (!await labelRepository.SaveChangesAsync(cancellationToken))
        {
            return new RenameLabelResult(RenameLabelStatus.DuplicateName);
        }

        return new RenameLabelResult(
            RenameLabelStatus.Succeeded,
            new LabelResult(label.Id, label.Name, label.CreatedAtUtc));
    }

    public Task<bool> DeleteAsync(
        DeleteLabelCommand command,
        CancellationToken cancellationToken)
    {
        return labelRepository.DeleteAsync(
            command.LabelId,
            command.UserId,
            cancellationToken);
    }
}
