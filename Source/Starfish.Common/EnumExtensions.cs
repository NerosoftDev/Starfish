using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

internal static class EnumExtensions
{
	/// <summary>
	/// 获取枚举字段描述
	/// </summary>
	/// <param name="enum"></param>
	/// <param name="resourceManager"></param>
	/// <param name="resourceCulture"></param>
	/// <returns></returns>
	/// <exception cref="NullReferenceException"></exception>
	public static string GetDescription(this Enum @enum, ResourceManager resourceManager, CultureInfo resourceCulture)
	{
		var field = @enum.GetType().GetField(@enum.ToString());
		if (field == null)
		{
			throw new NullReferenceException($"Field ‘{@enum}’ not defined.");
		}

		var attribute = field.GetCustomAttribute<DescriptionAttribute>();
		var key = attribute?.Description ?? @enum.ToString();
		var value = resourceManager.GetString(key, resourceCulture);
		return value;
	}
}