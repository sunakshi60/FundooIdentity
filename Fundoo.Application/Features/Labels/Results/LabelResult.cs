namespace Fundoo.Application.Features.Labels.Results;

public sealed record LabelResult(
    Guid Id,
    string Name,
    DateTimeOffset CreatedAtUtc);
