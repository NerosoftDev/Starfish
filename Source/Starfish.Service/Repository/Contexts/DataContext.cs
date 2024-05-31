using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Domain;
using Nerosoft.Euonia.Modularity;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 数据上下文
/// </summary>
public sealed class DataContext : DataContextBase<DataContext>
{
	private readonly IModelBuilder _builder;
	private readonly IBus _bus;
	private readonly IRequestContextAccessor _request;

	/// <summary>
	/// 初始化<see cref="DataContext"/>.
	/// </summary>
	/// <param name="options"></param>
	/// <param name="builder"></param>
	/// <param name="bus"></param>
	public DataContext(DbContextOptions<DataContext> options, IModelBuilder builder, IBus bus)
		: base(options)
	{
		_builder = builder;
		_bus = bus;
	}

	/// <summary>
	/// 初始化<see cref="DataContext"/>.
	/// </summary>
	/// <param name="options"></param>
	/// <param name="builder"></param>
	/// <param name="bus"></param>
	/// <param name="request"></param>
	public DataContext(DbContextOptions<DataContext> options, IModelBuilder builder, IBus bus, IRequestContextAccessor request)
		: this(options, builder, bus)
	{
		_request = request;
	}

	/// <inheritdoc/>
	protected override bool AutoSetEntryValues => true;

	/// <summary>
	/// 时间类型
	/// </summary>
	protected override DateTimeKind DateTimeKind => DateTimeKind.Local;

	/// <inheritdoc />
	public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
	{
		var entries = ChangeTracker.Entries().ToList();

		foreach (var entry in entries)
		{
			if (entry.Entity is not IAuditing auditing)
			{
				continue;
			}

			switch (entry.State)
			{
				case EntityState.Unchanged:
					continue;
				case EntityState.Added:
					auditing.UpdatedBy = auditing.CreatedBy = _request.Context.User?.Identity?.Name;
					break;
				case EntityState.Modified:
					auditing.UpdatedBy = _request.Context.User?.Identity?.Name;
					break;
				case EntityState.Deleted:
					auditing.UpdatedBy = _request.Context.User?.Identity?.Name;
					break;
			}
		}

		var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

		var events = GetTrackedEvents();

		if (result > 0 && events.Count > 0)
		{
			var options = new PublishOptions
			{
				RequestTraceId = _request.Context.TraceIdentifier
			};
			foreach (var @event in events)
			{
				await _bus.PublishAsync(@event, options, null, cancellationToken);
			}
		}

		{
		}

		return result;
	}

	/// <inheritdoc/>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		_builder.Configure(modelBuilder);
		modelBuilder.SetTombstoneQueryFilter();
		base.OnModelCreating(modelBuilder);
	}

	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		base.ConfigureConventions(configurationBuilder);
		configurationBuilder.Properties<DateTime>()
		                    .HaveConversion<DateTimeStorageConverter>();
		configurationBuilder.Properties<DateTime?>()
		                    .HaveConversion<DateTimeStorageConverter>();
	}

	private List<DomainEvent> GetTrackedEvents()
	{
		var entries = ChangeTracker.Entries<IHasDomainEvents>();

		var events = new List<DomainEvent>();

		foreach (var entry in entries)
		{
			var aggregate = entry.Entity;

			aggregate.AttachToEvents();
			events.AddRange(aggregate.GetEvents());
			aggregate.ClearEvents();
		}

		return events;
	}
}