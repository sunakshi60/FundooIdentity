namespace Fundoo.Domain.Entities;

public sealed class NoteLabel
{
    public Guid NoteId { get; set; }

    public Note Note { get; set; } = null!;

    public Guid LabelId { get; set; }

    public Label Label { get; set; } = null!;
}
