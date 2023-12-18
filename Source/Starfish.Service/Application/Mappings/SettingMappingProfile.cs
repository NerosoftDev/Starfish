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
		CreateMap<SettingNode, SettingNodeItemDto>();
		CreateMap<SettingNode, SettingNodeDetailDto>();
	}
}