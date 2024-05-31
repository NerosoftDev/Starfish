using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Nerosoft.Starfish.Repository;

public class DateTimeStorageConverter : ValueConverter<DateTime, DateTime>
{
	public DateTimeStorageConverter()
		: base(t => ConvertToUniversalTime(t), t => ConvertToLocalTime(t))
	{
	}

	private static DateTime ConvertToUniversalTime(DateTime time)
	{
		return time.Kind switch
		{
			DateTimeKind.Unspecified => DateTime.SpecifyKind(time, DateTimeKind.Local).ToUniversalTime(),
			DateTimeKind.Local => time.ToUniversalTime(),
			_ => time
		};
	}

	private static DateTime ConvertToLocalTime(DateTime time)
	{
		return time.Kind switch
		{
			DateTimeKind.Unspecified => DateTime.SpecifyKind(time, DateTimeKind.Utc).ToLocalTime(),
			DateTimeKind.Utc => time.ToLocalTime(),
			_ => time
		};
	}
}