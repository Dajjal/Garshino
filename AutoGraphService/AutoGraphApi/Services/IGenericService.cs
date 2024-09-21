using Ardalis.Specification;

namespace AutoGraphApi.Services;

public interface IGenericService<TEntity>
{
    Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default);
    Task<List<TEntity>> ListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(Guid id, TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}