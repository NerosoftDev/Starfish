using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Nerosoft.Starfish.Repository;

internal class UniversalTimeValueGenerator : ValueGenerator<DateTime>
{
	public override DateTime Next(EntityEntry entry)
	{
		return DateTime.UtcNow;
	}

	public override bool GeneratesTemporaryValues => false;
}