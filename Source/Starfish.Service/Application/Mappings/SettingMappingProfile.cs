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
		CreateMap<SettingNode, SettingNodeItemDto>()
			.ForMember(dest => dest.Type, options => options.MapFrom(src => GetSettingNodeTypeDescription(src.Type)))
			.ForMember(dest => dest.IsRoot, options => options.MapFrom(src => src.Type == SettingNodeType.Root));
		CreateMap<SettingNode, SettingNodeDetailDto>()
			.ForMember(dest => dest.Type, options => options.MapFrom(src => GetSettingNodeTypeDescription(src.Type)))
			.ForMember(dest => dest.IsRoot, options => options.MapFrom(src => src.Type == SettingNodeType.Root));
	}

	private static string GetSettingNodeTypeDescription(SettingNodeType type)
	{
		return type switch
		{
			SettingNodeType.Root => Resources.IDS_ENUM_SETTING_NODE_TYPE_ROOT,
			SettingNodeType.Object => Resources.IDS_ENUM_SETTING_NODE_TYPE_OBJECT,
			SettingNodeType.Array => Resources.IDS_ENUM_SETTING_NODE_TYPE_ARRAY,
			SettingNodeType.String => Resources.IDS_ENUM_SETTING_NODE_TYPE_STRING,
			SettingNodeType.Number => Resources.IDS_ENUM_SETTING_NODE_TYPE_NUMBER,
			SettingNodeType.Boolean => Resources.IDS_ENUM_SETTING_NODE_TYPE_BOOLEAN,
			SettingNodeType.Referer => Resources.IDS_ENUM_SETTING_NODE_TYPE_REFERER,
			_ => type.ToString()
		};
	}
}