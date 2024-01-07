using AutoMapper;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户映射配置
/// </summary>
internal class UserMappingProfile : Profile
{
	/// <summary>
	/// 
	/// </summary>
	public UserMappingProfile()
	{
		CreateMap<User, UserItemDto>()
			.ForMember(dest => dest.Roles, opt => opt.MapFrom(GetRoles));
		CreateMap<User, UserDetailDto>()
			.ForMember(dest => dest.Roles, opt => opt.MapFrom(GetRoles));
		CreateMap<UserCreateDto, UserCreateCommand>();
		CreateMap<UserUpdateDto, UserUpdateCommand>();
	}

	private static List<string> GetRoles(User source, UserItemDto destination)
	{
		return source.Roles.Select(t => t.Name).ToList();
	}

	private static List<string> GetRoles(User source, UserDetailDto destination)
	{
		return source.Roles.Select(t => t.Name).ToList();
	}
}