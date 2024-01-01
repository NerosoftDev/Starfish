using AutoMapper;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置节点映射配置
/// </summary>
public class SettingMappingProfile : Profile
{
	/// <inheritdoc />
	public SettingMappingProfile()
	{
		CreateMap<Setting, SettingItemDto>()
			.ForMember(dest => dest.StatusDescription, options => options.MapFrom(src => GetStatusDescription(src.Status)))
			.ForMember(dest => dest.AppName, options => options.MapFrom(src => src.App.Name));
		CreateMap<Setting, SettingDetailDto>()
			.ForMember(dest => dest.StatusDescription, options => options.MapFrom(src => GetStatusDescription(src.Status)))
			.ForMember(dest => dest.AppName, options => options.MapFrom(src => src.App.Name));
	}

	private static string GetStatusDescription(SettingStatus status)
	{
		return status switch
		{
			SettingStatus.Disabled => Resources.IDS_ENUM_SETTING_STATUS_DISABLED,
			SettingStatus.Pending => Resources.IDS_ENUM_SETTING_STATUS_PENDING,
			SettingStatus.Published => Resources.IDS_ENUM_SETTING_STATUS_PUBLISHED,
			_ => status.ToString()
		};
	}
}