using Fundoo.Application.Features.Notes.Commands;
using Fundoo.Application.Features.Notes.Results;
using Fundoo.Application.Features.Notes.Queries;

namespace Fundoo.Application.Features.Notes.Services;

public interface INoteService
{
    Task<CreateNoteResult> CreateAsync(
        CreateNoteCommand command,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<NoteListItemResult>> GetByUserAsync(
        GetNotesQuery query,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<NoteListItemResult>> GetTrashedByUserAsync(
        GetTrashedNotesQuery query,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<NoteListItemResult>> GetArchivedByUserAsync(
        GetArchivedNotesQuery query,
        CancellationToken cancellationToken);

    Task<NoteListItemResult?> GetByIdForUserAsync(
        GetNoteByIdQuery query,
        CancellationToken cancellationToken);

    Task<UpdateNoteResult?> UpdateAsync(
        UpdateNoteCommand command,
        CancellationToken cancellationToken);

    Task<bool> TrashAsync(
        TrashNoteCommand command,
        CancellationToken cancellationToken);

    Task<bool> RestoreAsync(
        RestoreNoteCommand command,
        CancellationToken cancellationToken);

    Task<bool> SetArchivedAsync(
        SetNoteArchiveStatusCommand command,
        CancellationToken cancellationToken);

    Task<bool> SetLabelAssociationAsync(
        SetNoteLabelAssociationCommand command,
        CancellationToken cancellationToken);
}
