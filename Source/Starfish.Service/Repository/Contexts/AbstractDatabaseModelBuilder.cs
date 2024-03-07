using Microsoft.EntityFrameworkCore;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

internal abstract class AbstractDatabaseModelBuilder : IModelBuilder
{
	public virtual void Configure(ModelBuilder modelBuilder)
	{
		ConfigureUser(modelBuilder);
		ConfigureTeam(modelBuilder);
		ConfigureConfiguration(modelBuilder);
		ConfigureSupported(modelBuilder);
	}

	protected abstract ModelBuilder ConfigureUser(ModelBuilder modelBuilder);
	
	protected abstract ModelBuilder ConfigureTeam(ModelBuilder modelBuilder);

	protected abstract ModelBuilder ConfigureConfiguration(ModelBuilder modelBuilder);

	protected abstract ModelBuilder ConfigureSupported(ModelBuilder modelBuilder);
}