namespace Fundoo.Application.Features.Notes.Results;

using Fundoo.Application.Features.Labels.Results;

public sealed record NoteListItemResult(
    Guid Id,
    string Title,
    string Content,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc,
    bool IsArchived,
    bool IsTrashed,
    IReadOnlyList<LabelResult> Labels);
