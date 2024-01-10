using AutoMapper;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用信息映射配置
/// </summary>
internal class AppsMappingProfile : Profile
{
	/// <summary>
	/// 构造函数
	/// </summary>
	public AppsMappingProfile()
	{
		CreateMap<AppInfo, AppInfoItemDto>()
			.ForMember(dest => dest.StatusDescription, opt => opt.MapFrom(src => src.Status.GetDescription(Resources.ResourceManager, Resources.Culture)));
		CreateMap<AppInfo, AppInfoDetailDto>()
			.ForMember(dest => dest.StatusDescription, opt => opt.MapFrom(src => src.Status.GetDescription(Resources.ResourceManager, Resources.Culture)));
	}

	private static string Mask(string source)
	{
		return $"{source[..3]}******{source[^3..]}";
	}
}