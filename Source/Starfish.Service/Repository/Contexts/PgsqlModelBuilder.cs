using Microsoft.EntityFrameworkCore;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

internal class PgsqlModelBuilder : IModelBuilder
{
	public void Configure(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>(entity =>
		{
			entity.ToTable("user");
		});
	}
}