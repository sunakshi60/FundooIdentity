using Fundoo.Domain.Entities;

namespace Fundoo.Application.Features.Labels.Repositories;

public interface ILabelRepository
{
    Task<bool> ExistsByNormalizedNameAsync(
        string userId,
        string normalizedName,
        CancellationToken cancellationToken);

    Task<bool> AddAsync(Label label, CancellationToken cancellationToken);

    Task<IReadOnlyList<Label>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken);

    Task<Label?> GetForUpdateByIdAsync(
        Guid labelId,
        string userId,
        CancellationToken cancellationToken);

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);

    Task<bool> DeleteAsync(
        Guid labelId,
        string userId,
        CancellationToken cancellationToken);
}
