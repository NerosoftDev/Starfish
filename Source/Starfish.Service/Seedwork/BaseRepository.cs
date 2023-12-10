using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Core;
using Nerosoft.Euonia.Domain;
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

	/// <summary>
	/// 根据指定的条件表达式查询数据并返回符合条件的数据集合
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="tracking"></param>
	/// <param name="properties"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool tracking, string[] properties, CancellationToken cancellationToken = default)
	{
		var query = Context.Set<TEntity>().AsQueryable();
		query = tracking ? query.AsTracking() : query.AsNoTracking();

		if (properties is { Length: > 0 })
		{
			query = properties.Aggregate(query, (current, property) => current.Include(property));
		}

		return query.Where(predicate).ToListAsync(cancellationToken);
	}

	/// <summary>
	/// 根据指定的条件表达式查询数据并返回符合条件的数据集合
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="tracking"></param>
	/// <param name="propertyAction"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool tracking, Func<IQueryable<TEntity>, IQueryable<TEntity>> propertyAction, CancellationToken cancellationToken = default)
	{
		var query = Context.Set<TEntity>().AsQueryable();
		query = tracking ? query.AsTracking() : query.AsNoTracking();

		if (propertyAction != null)
		{
			query = propertyAction(query);
		}

		return query.Where(predicate).ToListAsync(cancellationToken);
	}

	/// <summary>
	/// 根据指定的条件表达式查询数据并返回符合条件的数据集合
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="collator"></param>
	/// <param name="page"></param>
	/// <param name="size"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public virtual Task<List<TEntity>> FetchAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> collator, int page, int size, CancellationToken cancellationToken = default)
	{
		if (page <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(page), Resources.IDS_PAGE_NUMBER_MUST_GREATER_THAN_0);
		}

		if (size <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(size), Resources.IDS_PAGE_SIZE_MUST_GREATER_THAN_0);
		}

		var query = Context.Set<TEntity>().AsQueryable();
		if (collator != null)
		{
			query = collator(query);
		}

		query = query.Where(predicate).Skip((page - 1) * size).Take(size);
		return query.ToListAsync(cancellationToken);
	}

	/// <summary>
	/// 根据指定的条件表达式查询数量
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public override Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
	{
		var query = Context.Set<TEntity>().AsQueryable();
		return query.CountAsync(predicate, cancellationToken);
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
		var predicate = BuildIdEqualsExpression(id);

		//var lambda = predicate.Compile();
		return GetAsync(predicate, tracking, properties, cancellationToken);
	}

	/// <summary>
	/// 根据指定的Id查询数据
	/// </summary>
	/// <param name="id"></param>
	/// <param name="tracking"></param>
	/// <param name="propertyAction"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual Task<TEntity> GetAsync(TKey id, bool tracking, Func<IQueryable<TEntity>, IQueryable<TEntity>> propertyAction, CancellationToken cancellationToken = default)
	{
		var predicate = BuildIdEqualsExpression(id);

		return GetAsync(predicate, tracking, propertyAction, cancellationToken);
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
		var query = Context.Set<TEntity>().AsQueryable();
		query = tracking ? query.AsTracking() : query.AsNoTracking();

		if (properties is { Length: > 0 })
		{
			query = properties.Aggregate(query, (current, property) => current.Include(property));
		}

		return query.FirstOrDefaultAsync(predicate, cancellationToken);
	}

	/// <summary>
	/// 根据指定的条件表达式查询数据并返回符合条件的第一条数据
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="tracking"></param>
	/// <param name="propertyAction"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool tracking, Func<IQueryable<TEntity>, IQueryable<TEntity>> propertyAction, CancellationToken cancellationToken = default)
	{
		var query = Context.Set<TEntity>().AsQueryable();
		query = tracking ? query.AsTracking() : query.AsNoTracking();

		if (propertyAction != null)
		{
			query = propertyAction(query);
		}

		return query.FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
	}

	/// <summary>
	/// 根据Id集合查询数据并返回符合条件的数据集合
	/// </summary>
	/// <param name="ids"></param>
	/// <param name="tracking"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual Task<List<TEntity>> GetAsync(IEnumerable<TKey> ids, bool tracking, CancellationToken cancellationToken = default)
	{
		return GetAsync(ids, tracking, Array.Empty<string>(), cancellationToken);
	}

	/// <summary>
	/// 根据Id集合查询数据并返回符合条件的数据集合
	/// </summary>
	/// <param name="ids"></param>
	/// <param name="tracking"></param>
	/// <param name="properties"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual Task<List<TEntity>> GetAsync(IEnumerable<TKey> ids, bool tracking, string[] properties, CancellationToken cancellationToken = default)
	{
		var predicate = BuildIdInArrayExpression(ids);

		return FindAsync(predicate, tracking, properties, cancellationToken);
	}

	/// <summary>
	/// 根据Id集合查询数据并返回符合条件的数据集合
	/// </summary>
	/// <param name="ids"></param>
	/// <param name="tracking"></param>
	/// <param name="propertyAction"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual Task<List<TEntity>> GetAsync(IEnumerable<TKey> ids, bool tracking, Func<IQueryable<TEntity>, IQueryable<TEntity>> propertyAction, CancellationToken cancellationToken = default)
	{
		var predicate = BuildIdInArrayExpression(ids);

		return FindAsync(predicate, tracking, propertyAction, cancellationToken);
	}

	/// <summary>
	/// 检查符合条件的数据是否存在
	/// </summary>
	/// <param name="predicate">条件表达式</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
	{
		var query = Context.Set<TEntity>().AsQueryable();
		return query.AnyAsync(predicate, cancellationToken: cancellationToken);
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
		var predicate = BuildIdInArrayExpression(ids);
		var query = Context.Set<TEntity>().AsNoTracking()
		                   .Where(predicate)
		                   .Select(selector);

		var items = await query.ToListAsync(cancellationToken: cancellationToken);

		var result = items.ToDictionary(x => x.Key, x => x.Value);

		return result;
	}

	/// <summary>
	/// 更新实体
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="autoSave"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public override async Task UpdateAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default)
	{
		var set = Context.Set<TEntity>();
		set.Update(entity);
		if (autoSave)
		{
			await SaveChangesAsync(cancellationToken);
		}
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

		var entity = await set.FindAsync(id);
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
	/// 删除指定实体对象
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="autoSave"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public override async Task DeleteAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default)
	{
		var set = Context.Set<TEntity>();

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

	/// <summary>
	/// 获取实体Id
	/// </summary>
	/// <param name="entity"></param>
	/// <returns></returns>
	protected virtual TKey GetId(TEntity entity)
	{
		var property = Expression.PropertyOrField(Expression.Constant(entity), "Id");
		var lambda = Expression.Lambda<Func<TEntity, TKey>>(property, Expression.Parameter(typeof(TEntity), "entity")).Compile();
		return lambda(entity);
	}

	private static Expression<Func<TEntity, bool>> BuildIdEqualsExpression(TKey id)
	{
		// var parameter = Expression.Parameter(typeof(TEntity), "entity");
		// var member = Expression.PropertyOrField(parameter, "Id");
		// var expression = Expression.Call(typeof(object), nameof(Equals), new[] { member.Type }, member, Expression.Constant(id));
		// return Expression.Lambda<Func<TEntity, bool>>(expression, parameter);

		var parameter = Expression.Parameter(typeof(TEntity), "entity");
		var member = Expression.PropertyOrField(parameter, "Id");
		var expression = Expression.Equal(member, Expression.Constant(id, member.Type));
		var predicate = Expression.Lambda<Func<TEntity, bool>>(expression, parameter);
		return predicate;
	}

	private static Expression<Func<TEntity, bool>> BuildIdInArrayExpression(IEnumerable<TKey> ids)
	{
		var parameter = Expression.Parameter(typeof(TEntity), "entity");
		var member = Expression.PropertyOrField(parameter, "Id");
		var expression = Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), new[] { member.Type }, Expression.Constant(ids), member);
		var predicate = Expression.Lambda<Func<TEntity, bool>>(expression, parameter);
		return predicate;
	}

	/// <summary>
	/// 根据分页参数获计算跳过的数量
	/// </summary>
	/// <param name="page"></param>
	/// <param name="size"></param>
	/// <returns></returns>
	protected int GetSkipCount(int page, int size)
	{
		return (page - 1) * size;
	}
}