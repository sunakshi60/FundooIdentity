namespace Fundoo.Application.Features.Labels.Results;

public enum RenameLabelStatus
{
    Succeeded,
    NotFound,
    DuplicateName
}

public sealed record RenameLabelResult(
    RenameLabelStatus Status,
    LabelResult? Label = null);
