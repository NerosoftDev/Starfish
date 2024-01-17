using System.Linq.Expressions;
using Nerosoft.Euonia.Domain;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Service;

public interface IBaseRepository<TEntity, in TKey> : IRepository<TEntity, TKey>
	where TEntity : class, IEntity<TKey>
	where TKey : IEquatable<TKey>
{
	IQueryable<T> SetOf<T>()
		where T : class;

	Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> builder, CancellationToken cancellationToken = default);

	Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> builder, int page, int size, CancellationToken cancellationToken = default);

	Task<List<TEntity>> FindAsync(IEnumerable<TKey> keys, Func<IQueryable<TEntity>, IQueryable<TEntity>> builder, CancellationToken cancellationToken = default);

	Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> builder, CancellationToken cancellationToken = default);

	Task<TEntity> GetAsync(TKey key, Func<IQueryable<TEntity>, IQueryable<TEntity>> builder, CancellationToken cancellationToken = default);

	Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> builder, CancellationToken cancellationToken = default);

	Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> builder, CancellationToken cancellationToken = default);
}