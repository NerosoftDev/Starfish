using AutoMapper;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户映射配置
/// </summary>
public class UserMappingProfile : Profile
{
	/// <summary>
	/// 
	/// </summary>
	public UserMappingProfile()
	{
		CreateMap<User, UserItemDto>()
			.ForMember(dest => dest.Roles, opt => opt.MapFrom(src => ResolveUserRoles(src.Roles)));
		CreateMap<User, UserDetailDto>()
			.ForMember(dest => dest.Roles, opt => opt.MapFrom(src => ResolveUserRoles(src.Roles)));
		CreateMap<UserCreateDto, UserCreateCommand>();
		CreateMap<UserUpdateDto, UserUpdateCommand>();
	}

	private static List<string> ResolveUserRoles(string roles)
	{
		return roles.Split(",").ToList();
	}
}