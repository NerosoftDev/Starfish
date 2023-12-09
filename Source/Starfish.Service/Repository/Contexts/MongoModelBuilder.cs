using Microsoft.EntityFrameworkCore;
using Nerosoft.Starfish.Service;
using MongoDB.EntityFrameworkCore.Extensions;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

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