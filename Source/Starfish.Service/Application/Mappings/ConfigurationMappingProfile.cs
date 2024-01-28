using AutoMapper;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置节点映射配置
/// </summary>
internal class ConfigurationMappingProfile : Profile
{
	/// <inheritdoc />
	public ConfigurationMappingProfile()
	{
		CreateMap<ConfigurationItem, ConfigurationItemDto>();
		CreateMap<Configuration, ConfigurationDetailDto>()
			.ForMember(dest => dest.StatusDescription, options => options.MapFrom(src => GetStatusDescription(src.Status)))
			.ForMember(dest => dest.AppName, options => options.MapFrom(src => src.App.Name));
	}

	private static string GetStatusDescription(ConfigurationStatus status)
	{
		return status switch
		{
			ConfigurationStatus.Disabled => Resources.IDS_ENUM_CONFIG_STATUS_DISABLED,
			ConfigurationStatus.Pending => Resources.IDS_ENUM_CONFIG_STATUS_PENDING,
			ConfigurationStatus.Published => Resources.IDS_ENUM_CONFIG_STATUS_PUBLISHED,
			_ => status.ToString()
		};
	}
}