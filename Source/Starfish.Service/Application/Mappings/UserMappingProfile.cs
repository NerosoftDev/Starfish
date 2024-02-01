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
		CreateMap<User, UserItemDto>();
		CreateMap<User, UserDetailDto>();
		CreateMap<UserCreateDto, UserCreateCommand>();
		CreateMap<UserUpdateDto, UserUpdateCommand>();
	}
}