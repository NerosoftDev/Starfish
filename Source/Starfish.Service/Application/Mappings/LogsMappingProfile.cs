using AutoMapper;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;
using Newtonsoft.Json;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 日志映射配置
/// </summary>
internal class LogsMappingProfile : Profile
{
	/// <summary>
	/// 初始化日志映射配置
	/// </summary>
	public LogsMappingProfile()
	{
		CreateMap<OperateLog, OperateLogDto>()
			.ForMember(dest => dest.Description, opt => opt.MapFrom(GetDescription));
	}

	private static string GetDescription(OperateLog source, OperateLogDto destination, object obj, ResolutionContext context)
	{
		var key = $"IDS_MESSAGE_LOGS_{source.Module}_{source.Type}".Normalize(TextCaseType.Upper).Replace(".", "_");

		var value = Resources.ResourceManager.GetString(key);

		if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(source.Content))
		{
			return value;
		}

		try
		{
			var args = JsonConvert.DeserializeObject<object[]>(source.Content);

			return string.Format(value, args);
		}
		catch
		{
			return value;
		}
	}
}