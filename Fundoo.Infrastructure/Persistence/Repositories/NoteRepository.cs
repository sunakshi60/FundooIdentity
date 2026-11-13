using Fundoo.Application.Features.Notes.Repositories;
using Fundoo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Fundoo.Infrastructure.Persistence.Repositories;

public sealed class NoteRepository(FundooDbContext dbContext) : INoteRepository
{
    public async Task AddAsync(Note note, CancellationToken cancellationToken)
    {
        dbContext.Notes.Add(note);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Note>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        return await dbContext.Notes
            .AsNoTracking()
            .Include(note => note.NoteLabels)
                .ThenInclude(noteLabel => noteLabel.Label)
            .Where(note => note.UserId == userId && !note.IsTrashed && !note.IsArchived)
            .OrderByDescending(note => note.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Note>> GetArchivedByUserIdAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        return await dbContext.Notes
            .AsNoTracking()
            .Include(note => note.NoteLabels)
                .ThenInclude(noteLabel => noteLabel.Label)
            .Where(note => note.UserId == userId && note.IsArchived && !note.IsTrashed)
            .OrderByDescending(note => note.UpdatedAtUtc ?? note.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Note>> GetTrashedByUserIdAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        return await dbContext.Notes
            .AsNoTracking()
            .Include(note => note.NoteLabels)
                .ThenInclude(noteLabel => noteLabel.Label)
            .Where(note => note.UserId == userId && note.IsTrashed)
            .OrderByDescending(note => note.UpdatedAtUtc ?? note.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public Task<Note?> GetByIdForUserAsync(
        Guid noteId,
        string userId,
        CancellationToken cancellationToken)
    {
        return dbContext.Notes
            .AsNoTracking()
            .Include(note => note.NoteLabels)
                .ThenInclude(noteLabel => noteLabel.Label)
            .FirstOrDefaultAsync(
                note => note.Id == noteId &&
                        note.UserId == userId &&
                        !note.IsTrashed,
                cancellationToken);
    }

    public Task<Note?> GetForUpdateByIdAsync(
        Guid noteId,
        string userId,
        CancellationToken cancellationToken)
    {
        return dbContext.Notes
            .FirstOrDefaultAsync(
                note => note.Id == noteId &&
                        note.UserId == userId &&
                        !note.IsTrashed,
                cancellationToken);
    }

    public Task<Note?> GetTrashedForUpdateByIdAsync(
        Guid noteId,
        string userId,
        CancellationToken cancellationToken)
    {
        return dbContext.Notes
            .FirstOrDefaultAsync(
                note => note.Id == noteId &&
                        note.UserId == userId &&
                        note.IsTrashed,
                cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> SetLabelAssociationAsync(
        Guid noteId,
        Guid labelId,
        string userId,
        bool isAttached,
        CancellationToken cancellationToken)
    {
        var noteExists = await dbContext.Notes.AnyAsync(
            note => note.Id == noteId && note.UserId == userId && !note.IsTrashed,
            cancellationToken);
        var labelExists = await dbContext.Labels.AnyAsync(
            label => label.Id == labelId && label.UserId == userId,
            cancellationToken);

        if (!noteExists || !labelExists)
        {
            return false;
        }

        var association = await dbContext.NoteLabels.FirstOrDefaultAsync(
            item => item.NoteId == noteId && item.LabelId == labelId,
            cancellationToken);

        if (isAttached)
        {
            if (association is not null)
            {
                return true;
            }

            association = new NoteLabel { NoteId = noteId, LabelId = labelId };
            dbContext.NoteLabels.Add(association);

            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException exception) when (IsAssociationConflict(exception))
            {
                dbContext.Entry(association).State = EntityState.Detached;
            }

            return true;
        }

        if (association is not null)
        {
            dbContext.NoteLabels.Remove(association);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return true;
    }

    private static bool IsAssociationConflict(DbUpdateException exception)
    {
        return exception.InnerException is PostgresException
        {
            SqlState: PostgresErrorCodes.UniqueViolation,
            ConstraintName: "PK_NoteLabels"
        };
    }
}
