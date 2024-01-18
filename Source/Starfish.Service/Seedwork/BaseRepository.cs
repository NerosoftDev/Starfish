using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Domain;
using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Repository;
using Nerosoft.Euonia.Repository.EfCore;

// ReSharper disable UnusedMember.Global

// ReSharper disable MemberCanBeProtected.Global

namespace Nerosoft.Starfish.Service;

/// <summary>
/// 数据仓库基类
/// </summary>
/// <typeparam name="TContext"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class BaseRepository<TContext, TEntity, TKey> : EfCoreRepository<TContext, TEntity, TKey>,
                                                                IBaseRepository<TContext, TEntity, TKey>,
                                                                ITransientDependency
	where TContext : DbContext, IRepositoryContext
	where TEntity : class, IEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <summary>
	/// 初始化<see cref="BaseRepository{TContext, TEntity, TKey}"/>
	/// </summary>
	/// <param name="provider"></param>
	protected BaseRepository(IContextProvider provider)
		: base(provider)
	{
	}

	public IQueryable<T> SetOf<T>()
		where T : class
	{
		return Context.SetOf<T>();
	}

	/// <summary>
	/// 根据指定的Id查询数据
	/// </summary>
	/// <param name="id"></param>
	/// <param name="tracking"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual Task<TEntity> GetAsync(TKey id, bool tracking, CancellationToken cancellationToken = default)
	{
		//var lambda = predicate.Compile();
		return GetAsync(id, tracking, Array.Empty<string>(), cancellationToken);
	}

	/// <summary>
	/// 根据指定的Id查询数据
	/// </summary>
	/// <param name="id"></param>
	/// <param name="tracking"></param>
	/// <param name="properties"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual Task<TEntity> GetAsync(TKey id, bool tracking, string[] properties, CancellationToken cancellationToken = default)
	{
		var predicate = PredicateBuilder.PropertyEqual<TEntity, TKey>(nameof(IEntity<TKey>.Id), id);

		//var lambda = predicate.Compile();
		return GetAsync(predicate, tracking, properties, cancellationToken);
	}

	/// <summary>
	/// 根据指定的条件表达式查询数据并返回符合条件的第一条数据
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="tracking"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool tracking, CancellationToken cancellationToken = default)
	{
		return GetAsync(predicate, tracking, Array.Empty<string>(), cancellationToken);
	}

	/// <summary>
	/// 根据指定的条件表达式查询数据并返回符合条件的第一条数据
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="tracking"></param>
	/// <param name="properties"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool tracking, string[] properties, CancellationToken cancellationToken = default)
	{
		return base.GetAsync(predicate, query => BuildQuery(query, tracking, properties), cancellationToken);
	}

	/// <summary>
	/// 根据Id集合查询数据并返回符合条件的数据集合
	/// </summary>
	/// <param name="ids"></param>
	/// <param name="properties"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual Task<List<TEntity>> FindAsync(IEnumerable<TKey> ids, string[] properties, CancellationToken cancellationToken = default)
	{
		var predicate = PredicateBuilder.PropertyInRange<TEntity, TKey>(nameof(IEntity<TKey>.Id), ids.ToArray());
		return FindAsync(predicate, properties, cancellationToken);
	}

	/// <summary>
	/// 根据指定的条件表达式查询数据并返回符合条件的数据集合
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="properties"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, string[] properties, CancellationToken cancellationToken = default)
	{
		return base.FindAsync(predicate, query => BuildQuery(query, false, properties), cancellationToken);
	}

	/// <summary>
	/// 根据Id集合查询指定字段
	/// </summary>
	/// <param name="ids"></param>
	/// <param name="selector"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual async Task<Dictionary<TKey, string>> LookupAsync(IEnumerable<TKey> ids, Expression<Func<TEntity, KeyValuePair<TKey, string>>> selector, CancellationToken cancellationToken = default)
	{
		var predicate = PredicateBuilder.PropertyInRange<TEntity, TKey>(nameof(IEntity<TKey>.Id), ids.ToArray());
		var query = Context.Set<TEntity>().AsNoTracking()
		                   .Where(predicate)
		                   .Select(selector);

		var items = await query.ToListAsync(cancellationToken: cancellationToken);

		var result = items.ToDictionary(x => x.Key, x => x.Value);

		return result;
	}

	/// <summary>
	/// 根据Id删除实体
	/// </summary>
	/// <param name="id"></param>
	/// <param name="autoSave"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	public virtual async Task DeleteAsync(TKey id, bool autoSave = true, CancellationToken cancellationToken = default)
	{
		var set = Context.Set<TEntity>();

		var entity = await set.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
		if (entity is null)
		{
			throw new NotFoundException();
		}

		set.Remove(entity);
		if (autoSave)
		{
			await SaveChangesAsync(cancellationToken);
		}
	}

	/// <summary>
	/// 根据Id删除实体
	/// </summary>
	/// <typeparam name="TEvent"></typeparam>
	/// <param name="id"></param>
	/// <param name="eventFactory"></param>
	/// <param name="autoSave"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	public virtual async Task DeleteAsync<TEvent>(TKey id, Func<TEvent> eventFactory, bool autoSave = true, CancellationToken cancellationToken = default)
		where TEvent : DomainEvent
	{
		var set = Context.Set<TEntity>();

		var entity = await set.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
		switch (entity)
		{
			case null:
				throw new NotFoundException();
			case IHasDomainEvents aggregate:
			{
				var @event = eventFactory();
				aggregate.RaiseEvent(@event);
				break;
			}
		}

		set.Remove(entity);
		if (autoSave)
		{
			await SaveChangesAsync(cancellationToken);
		}
	}

	/// <summary>
	/// 保存更改
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return Context.SaveChangesAsync(cancellationToken);
	}

	/// <summary>
	/// 删除指定实体对象
	/// </summary>
	/// <typeparam name="TEvent"></typeparam>
	/// <param name="entity"></param>
	/// <param name="eventFactory"></param>
	/// <param name="autoSave"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual Task DeleteAsync<TEvent>(TEntity entity, Func<TEvent> eventFactory, bool autoSave = true, CancellationToken cancellationToken = default)
		where TEvent : DomainEvent
	{
		if (entity is IHasDomainEvents aggregate)
		{
			var @event = eventFactory();
			aggregate.RaiseEvent(@event);
		}

		return DeleteAsync(entity, autoSave, cancellationToken);
	}

	protected virtual IQueryable<TEntity> BuildQuery(IQueryable<TEntity> query, bool tracking, string[] properties)
	{
		query = tracking ? query.AsTracking() : query.AsNoTracking();

		if (properties is { Length: > 0 })
		{
			query = properties.Aggregate(query, (current, property) => current.Include(property));
		}

		return query;
	}
}