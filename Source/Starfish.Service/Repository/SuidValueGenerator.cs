using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Nerosoft.Starfish.Repository;

internal class SuidValueGenerator : ValueGenerator<string>
{
	public override string Next(EntityEntry entry)
	{
		var snowflake = ObjectId.NewSnowflake();
		return ShortUniqueId.Default.EncodeInt64(snowflake);
	}

	public override bool GeneratesTemporaryValues => false;
}