using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

public sealed class DataContext : DataContextBase<DataContext>
{
	private readonly IModelBuilder _modelBuilder;
	public DataContext(DbContextOptions<DataContext> options, IModelBuilder modelBuilder, ILoggerFactory factory)
		: base(options, factory)
	{
		_modelBuilder = modelBuilder;
	}

	protected override bool AutoSetEntryValues { get; }

	protected override bool EnabledPublishEvents { get; }

	/// <inheritdoc/>
	protected override Task PublishEventAsync<TEvent>(TEvent @event)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc/>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		_modelBuilder.Configure(modelBuilder);
	}
}
