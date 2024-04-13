using AutoMapper;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

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
		var key = $"IDS_LOG_MESSAGE_{source.Module}_{source.Type}".Normalize(TextCaseType.Upper).Replace(".", "_");

		var value = Resources.ResourceManager.GetString(key);

		if (string.IsNullOrEmpty(value))
		{
			return value;
		}

		return string.Format(value, source.Content);
	}
}