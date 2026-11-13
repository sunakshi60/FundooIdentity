namespace Fundoo.Application.Features.Notes.Results;

public sealed record UpdateNoteResult(
    Guid Id,
    string Title,
    string Content,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc,
    bool IsArchived,
    bool IsTrashed);
