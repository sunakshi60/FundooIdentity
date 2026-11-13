namespace Fundoo.Application.Features.Labels.Results;

public sealed record CreateLabelResult(
    bool Succeeded,
    LabelResult? Label,
    string? ErrorMessage);
