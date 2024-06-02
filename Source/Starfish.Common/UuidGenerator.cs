namespace Nerosoft.Starfish.Common;

internal class UuidGenerator
{
	public static string New()
	{
		var bytes = Guid.NewGuid().ToByteArray();
		var longValue = BitConverter.ToInt64(bytes, 0);
		var shortUuid = Base62Encode(longValue);
		return shortUuid;
	}

	private static string Base62Encode(long value)
	{
		const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
		var result = string.Empty;

		do
		{
			result = chars[(int)(value % 62)] + result;
			value /= 62;
		}
		while (value > 0);

		return result;
	}
}