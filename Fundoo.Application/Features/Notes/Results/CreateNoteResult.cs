namespace Fundoo.Application.Features.Notes.Results;

public sealed record CreateNoteResult(
    Guid Id,
    string Title,
    string Content,
    DateTimeOffset CreatedAtUtc);
