using Fundoo.Application.Features.Notes.Commands;
using Fundoo.Application.Features.Notes.Queries;
using Fundoo.Application.Features.Notes.Repositories;
using Fundoo.Application.Features.Notes.Results;
using Fundoo.Application.Features.Notes.Services;
using Fundoo.Domain.Entities;

namespace Fundoo.Infrastructure.Services.Notes;

public sealed class NoteService(
    INoteRepository noteRepository,
    TimeProvider timeProvider) : INoteService
{
    public async Task<CreateNoteResult> CreateAsync(
        CreateNoteCommand command,
        CancellationToken cancellationToken)
    {
        var note = new Note
        {
            UserId = command.UserId,
            Title = command.Title.Trim(),
            Content = command.Content ?? string.Empty
        };

        await noteRepository.AddAsync(note, cancellationToken);

        return new CreateNoteResult(
            note.Id,
            note.Title,
            note.Content,
            note.CreatedAtUtc);
    }

    public async Task<IReadOnlyList<NoteListItemResult>> GetByUserAsync(
        GetNotesQuery query,
        CancellationToken cancellationToken)
    {
        var notes = await noteRepository.GetByUserIdAsync(
            query.UserId,
            cancellationToken);

        return notes
            .Select(ToNoteListItemResult)
            .ToArray();
    }

    public async Task<IReadOnlyList<NoteListItemResult>> GetTrashedByUserAsync(
        GetTrashedNotesQuery query,
        CancellationToken cancellationToken)
    {
        var notes = await noteRepository.GetTrashedByUserIdAsync(
            query.UserId,
            cancellationToken);

        return notes
            .Select(ToNoteListItemResult)
            .ToArray();
    }

    public async Task<IReadOnlyList<NoteListItemResult>> GetArchivedByUserAsync(
        GetArchivedNotesQuery query,
        CancellationToken cancellationToken)
    {
        var notes = await noteRepository.GetArchivedByUserIdAsync(
            query.UserId,
            cancellationToken);

        return notes
            .Select(ToNoteListItemResult)
            .ToArray();
    }

    public async Task<NoteListItemResult?> GetByIdForUserAsync(
        GetNoteByIdQuery query,
        CancellationToken cancellationToken)
    {
        var note = await noteRepository.GetByIdForUserAsync(
            query.NoteId,
            query.UserId,
            cancellationToken);

        return note is null ? null : ToNoteListItemResult(note);
    }

    public async Task<UpdateNoteResult?> UpdateAsync(
        UpdateNoteCommand command,
        CancellationToken cancellationToken)
    {
        var note = await noteRepository.GetForUpdateByIdAsync(
            command.NoteId,
            command.UserId,
            cancellationToken);

        if (note is null)
        {
            return null;
        }

        note.Title = command.Title.Trim();
        note.Content = command.Content ?? string.Empty;
        note.UpdatedAtUtc = timeProvider.GetUtcNow();

        await noteRepository.SaveChangesAsync(cancellationToken);

        return new UpdateNoteResult(
            note.Id,
            note.Title,
            note.Content,
            note.CreatedAtUtc,
            note.UpdatedAtUtc,
            note.IsArchived,
            note.IsTrashed);
    }

    public async Task<bool> TrashAsync(
        TrashNoteCommand command,
        CancellationToken cancellationToken)
    {
        var note = await noteRepository.GetForUpdateByIdAsync(
            command.NoteId,
            command.UserId,
            cancellationToken);

        if (note is null)
        {
            return false;
        }

        note.IsTrashed = true;
        note.UpdatedAtUtc = timeProvider.GetUtcNow();
        await noteRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> RestoreAsync(
        RestoreNoteCommand command,
        CancellationToken cancellationToken)
    {
        var note = await noteRepository.GetTrashedForUpdateByIdAsync(
            command.NoteId,
            command.UserId,
            cancellationToken);

        if (note is null)
        {
            return false;
        }

        note.IsTrashed = false;
        note.UpdatedAtUtc = timeProvider.GetUtcNow();
        await noteRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> SetArchivedAsync(
        SetNoteArchiveStatusCommand command,
        CancellationToken cancellationToken)
    {
        var note = await noteRepository.GetForUpdateByIdAsync(
            command.NoteId,
            command.UserId,
            cancellationToken);

        if (note is null)
        {
            return false;
        }

        note.IsArchived = command.IsArchived;
        note.UpdatedAtUtc = timeProvider.GetUtcNow();
        await noteRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public Task<bool> SetLabelAssociationAsync(
        SetNoteLabelAssociationCommand command,
        CancellationToken cancellationToken)
    {
        return noteRepository.SetLabelAssociationAsync(
            command.NoteId,
            command.LabelId,
            command.UserId,
            command.IsAttached,
            cancellationToken);
    }

    private static NoteListItemResult ToNoteListItemResult(Note note)
    {
        var labels = note.NoteLabels
            .Select(noteLabel => new Fundoo.Application.Features.Labels.Results.LabelResult(
                noteLabel.Label.Id,
                noteLabel.Label.Name,
                noteLabel.Label.CreatedAtUtc))
            .ToArray();

        return new NoteListItemResult(
            note.Id,
            note.Title,
            note.Content,
            note.CreatedAtUtc,
            note.UpdatedAtUtc,
            note.IsArchived,
            note.IsTrashed,
            labels);
    }
}
