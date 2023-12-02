using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 数据上下文
/// </summary>
public sealed class DataContext : DataContextBase<DataContext>
{
	private readonly IModelBuilder _builder;
	private readonly IBus _bus;

	/// <summary>
	/// 初始化<see cref="DataContext"/>.
	/// </summary>
	/// <param name="options"></param>
	/// <param name="builder"></param>
	/// <param name="factory"></param>
	public DataContext(DbContextOptions<DataContext> options, IModelBuilder builder, ILoggerFactory factory)
		: base(options, factory)
	{
		_builder = builder;
	}

	/// <summary>
	/// 初始化<see cref="DataContext"/>.
	/// </summary>
	/// <param name="options"></param>
	/// <param name="modelBuilder"></param>
	/// <param name="bus"></param>
	/// <param name="factory"></param>
	public DataContext(DbContextOptions<DataContext> options, IModelBuilder modelBuilder, IBus bus, ILoggerFactory factory)
		: this(options, modelBuilder, factory)
	{
		_bus = bus;
	}

	/// <inheritdoc/>
	protected override bool AutoSetEntryValues { get; }

	/// <inheritdoc/>
	protected override bool EnabledPublishEvents { get; }

	/// <inheritdoc/>
	protected override async Task PublishEventAsync<TEvent>(TEvent @event)
	{
		await _bus.PublishAsync(@event);
	}

	/// <inheritdoc/>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		_builder.Configure(modelBuilder);
	}
}
