using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Nerosoft.Starfish.Repository;

internal class UuidValueGenerator : ValueGenerator<string>
{
	public override bool GeneratesTemporaryValues => false;

	public override string Next(EntityEntry entry)
	{
		return UuidGenerator.New();
	}
}
