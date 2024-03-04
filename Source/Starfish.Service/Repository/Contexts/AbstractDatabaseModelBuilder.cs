using Microsoft.EntityFrameworkCore;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

internal abstract class AbstractDatabaseModelBuilder : IModelBuilder
{
	public virtual void Configure(ModelBuilder modelBuilder)
	{
		ConfigureUser(modelBuilder);
		ConfigureTeam(modelBuilder);
		ConfigureTeamMember(modelBuilder);
		ConfigureConfiguration(modelBuilder);
		ConfigureConfigurationItem(modelBuilder);
		ConfigureConfigurationArchive(modelBuilder);
		ConfigureConfigurationRevision(modelBuilder);
		ConfigureToken(modelBuilder);
		ConfigureOperationLog(modelBuilder);
	}

	protected abstract ModelBuilder ConfigureUser(ModelBuilder modelBuilder);
	
	protected abstract ModelBuilder ConfigureTeam(ModelBuilder modelBuilder);

	protected abstract ModelBuilder ConfigureTeamMember(ModelBuilder modelBuilder);

	protected abstract ModelBuilder ConfigureConfiguration(ModelBuilder modelBuilder);

	protected abstract ModelBuilder ConfigureConfigurationItem(ModelBuilder modelBuilder);

	protected abstract ModelBuilder ConfigureConfigurationArchive(ModelBuilder modelBuilder);

	protected abstract ModelBuilder ConfigureConfigurationRevision(ModelBuilder modelBuilder);

	protected abstract ModelBuilder ConfigureToken(ModelBuilder modelBuilder);

	protected abstract ModelBuilder ConfigureOperationLog(ModelBuilder modelBuilder);
}