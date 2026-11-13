using Fundoo.Application.Features.Labels.Repositories;
using Fundoo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Fundoo.Infrastructure.Persistence.Repositories;

public sealed class LabelRepository(FundooDbContext dbContext) : ILabelRepository
{
    public Task<bool> ExistsByNormalizedNameAsync(
        string userId,
        string normalizedName,
        CancellationToken cancellationToken)
    {
        return dbContext.Labels.AnyAsync(
            label => label.UserId == userId && label.NormalizedName == normalizedName,
            cancellationToken);
    }

    public async Task<bool> AddAsync(Label label, CancellationToken cancellationToken)
    {
        dbContext.Labels.Add(label);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException exception) when (IsLabelNameConflict(exception))
        {
            dbContext.Entry(label).State = EntityState.Detached;
            return false;
        }
    }

    public async Task<IReadOnlyList<Label>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        return await dbContext.Labels
            .AsNoTracking()
            .Where(label => label.UserId == userId)
            .OrderBy(label => label.NormalizedName)
            .ToListAsync(cancellationToken);
    }

    public Task<Label?> GetForUpdateByIdAsync(
        Guid labelId,
        string userId,
        CancellationToken cancellationToken)
    {
        return dbContext.Labels.FirstOrDefaultAsync(
            label => label.Id == labelId && label.UserId == userId,
            cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException exception) when (IsLabelNameConflict(exception))
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(
        Guid labelId,
        string userId,
        CancellationToken cancellationToken)
    {
        var label = await dbContext.Labels.FirstOrDefaultAsync(
            item => item.Id == labelId && item.UserId == userId,
            cancellationToken);

        if (label is null)
        {
            return false;
        }

        dbContext.Labels.Remove(label);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static bool IsLabelNameConflict(DbUpdateException exception)
    {
        return exception.InnerException is PostgresException
        {
            SqlState: PostgresErrorCodes.UniqueViolation,
            ConstraintName: "IX_Labels_UserId_NormalizedName"
        };
    }
}
