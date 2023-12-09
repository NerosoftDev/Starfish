using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 数据上下文
/// </summary>
public sealed class DataContext : DataContextBase<DataContext>
{
	private readonly IModelBuilder _builder;

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

	/// <inheritdoc/>
	protected override bool AutoSetEntryValues => true;

	/// <inheritdoc/>
	protected override bool EnabledPublishEvents => false;

	/// <summary>
	/// 时间类型
	/// </summary>
	protected override DateTimeKind DateTimeKind => DateTimeKind.Local;

	/// <inheritdoc/>
	protected override Task PublishEventAsync<TEvent>(TEvent @event)
	{
		throw new NotSupportedException();
	}

	/// <inheritdoc/>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		_builder.Configure(modelBuilder);
		modelBuilder.SetTombstoneQueryFilter();
		base.OnModelCreating(modelBuilder);
	}
}