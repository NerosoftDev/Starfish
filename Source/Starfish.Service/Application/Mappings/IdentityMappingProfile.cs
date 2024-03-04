using AutoMapper;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户映射配置
/// </summary>
internal class IdentityMappingProfile : Profile
{
	/// <summary>
	/// 
	/// </summary>
	public IdentityMappingProfile()
	{
		CreateMap<User, UserItemDto>();
		CreateMap<User, UserDetailDto>();
		CreateMap<UserCreateDto, UserCreateCommand>();
		CreateMap<UserUpdateDto, UserUpdateCommand>();

		CreateMap<Team, TeamItemDto>();
		CreateMap<Team, TeamDetailDto>();

		CreateMap<TeamMember, TeamMemberDto>()
			.ForMember(dest => dest.UserName, options => options.MapFrom(src => src.User.UserName))
			.ForMember(dest => dest.NickName, options => options.MapFrom(src => src.User.NickName))
			.ForMember(dest => dest.Email, options => options.MapFrom(src => src.User.Email))
			.ForMember(dest => dest.Phone, options => options.MapFrom(src => src.User.Phone));
	}
	
	private static string Mask(string source)
	{
		return $"{source[..3]}******{source[^3..]}";
	}
}