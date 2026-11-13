namespace Fundoo.Domain.Entities;

public sealed class Label
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string UserId { get; set; }

    public required string Name { get; set; }

    public required string NormalizedName { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<NoteLabel> NoteLabels { get; set; } = new List<NoteLabel>();
}
