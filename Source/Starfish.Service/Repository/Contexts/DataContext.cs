using Microsoft.EntityFrameworkCore;
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
	public DataContext(DbContextOptions<DataContext> options, IModelBuilder builder)
		: base(options)
	{
		_builder = builder;
	}

	/// <inheritdoc/>
	protected override bool AutoSetEntryValues => true;

	/// <summary>
	/// 时间类型
	/// </summary>
	protected override DateTimeKind DateTimeKind => DateTimeKind.Local;

	/// <inheritdoc/>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		_builder.Configure(modelBuilder);
		modelBuilder.SetTombstoneQueryFilter();
		base.OnModelCreating(modelBuilder);
	}
}