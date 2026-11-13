using Fundoo.Domain.Entities;

namespace Fundoo.Application.Features.Notes.Repositories;

public interface INoteRepository
{
    Task AddAsync(Note note, CancellationToken cancellationToken);

    Task<IReadOnlyList<Note>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<Note>> GetTrashedByUserIdAsync(
        string userId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<Note>> GetArchivedByUserIdAsync(
        string userId,
        CancellationToken cancellationToken);

    Task<Note?> GetByIdForUserAsync(
        Guid noteId,
        string userId,
        CancellationToken cancellationToken);

    Task<Note?> GetForUpdateByIdAsync(
        Guid noteId,
        string userId,
        CancellationToken cancellationToken);

    Task<Note?> GetTrashedForUpdateByIdAsync(
        Guid noteId,
        string userId,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task<bool> SetLabelAssociationAsync(
        Guid noteId,
        Guid labelId,
        string userId,
        bool isAttached,
        CancellationToken cancellationToken);
}
