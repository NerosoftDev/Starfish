using Microsoft.EntityFrameworkCore;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

internal class MongoModelBuilder : IModelBuilder
{
	public void Configure(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>(entity =>
		{
			entity.ToTable("user");
		});
	}
}
