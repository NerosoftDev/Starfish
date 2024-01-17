using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Domain;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Service;

public interface IBaseRepository<out TContext, TEntity, TKey> : IRepository<TContext, TEntity, TKey>
	where TContext : DbContext, IRepositoryContext
	where TEntity : class, IEntity<TKey>
	where TKey : IEquatable<TKey>
{
	Task<TEntity> GetAsync(TKey id, bool tracking, CancellationToken cancellationToken = default);

	Task<TEntity> GetAsync(TKey id, bool tracking, string[] properties, CancellationToken cancellationToken = default);

	Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool tracking, CancellationToken cancellationToken = default);

	Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool tracking, string[] properties, CancellationToken cancellationToken = default);

	Task<List<TEntity>> FindAsync(IEnumerable<TKey> ids, string[] properties, CancellationToken cancellationToken = default);

	Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, string[] properties, CancellationToken cancellationToken = default);
	
	Task<Dictionary<TKey, string>> LookupAsync(IEnumerable<TKey> ids, Expression<Func<TEntity, KeyValuePair<TKey, string>>> selector, CancellationToken cancellationToken = default);

	Task DeleteAsync(TKey id, bool autoSave = true, CancellationToken cancellationToken = default);

	Task DeleteAsync<TEvent>(TKey id, Func<TEvent> eventFactory, bool autoSave = true, CancellationToken cancellationToken = default)
		where TEvent : DomainEvent;

	Task DeleteAsync<TEvent>(TEntity entity, Func<TEvent> eventFactory, bool autoSave = true, CancellationToken cancellationToken = default)
		where TEvent : DomainEvent;
}