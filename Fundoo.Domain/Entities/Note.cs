namespace Fundoo.Domain.Entities;

public sealed class Note
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string UserId { get; set; }

    public required string Title { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public bool IsArchived { get; set; }

    public bool IsTrashed { get; set; }

    public ICollection<NoteLabel> NoteLabels { get; set; } = new List<NoteLabel>();
}
