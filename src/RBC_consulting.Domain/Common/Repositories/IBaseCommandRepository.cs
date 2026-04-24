using RBC_consulting.Domain.Common.Entities;
using RBC_consulting.Domain.Common.Results;

namespace RBC_consulting.Domain.Common.Repositories;

public interface IBaseCommandRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : struct, IEquatable<TId>
{
    Task<Result<TId>> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> IsExistAsync(TId id, CancellationToken cancellationToken = default);
    Task<Result<TEntity>> GetAsync(TId id, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(TId id, CancellationToken cancellationToken = default);
}
