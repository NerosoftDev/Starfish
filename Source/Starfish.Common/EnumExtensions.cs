using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

internal static class EnumExtensions
{
	public static string GetDescription(this Enum @enum, ResourceManager resourceManager, CultureInfo resourceCulture)
	{
		var field = @enum.GetType().GetField(@enum.ToString());
		if (field == null)
		{
			throw new NullReferenceException("field");
		}
		var attribute = field.GetCustomAttribute<DescriptionAttribute>();
		var key = attribute?.Description ?? @enum.ToString();
		var value = resourceManager.GetString(key, resourceCulture);
		return value;
	}
}