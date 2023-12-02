using Microsoft.EntityFrameworkCore;
using Nerosoft.Starfish.Service;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Nerosoft.Starfish.Domain;

internal class MongoModelBuilder : IModelBuilder
{
	public void Configure(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>(entity =>
		{
			entity.ToCollection("user");
		});
	}
}
