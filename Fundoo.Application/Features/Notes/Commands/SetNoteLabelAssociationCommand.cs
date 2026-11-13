namespace Fundoo.Application.Features.Notes.Commands;

public sealed record SetNoteLabelAssociationCommand(
    Guid NoteId,
    Guid LabelId,
    string UserId,
    bool IsAttached);
