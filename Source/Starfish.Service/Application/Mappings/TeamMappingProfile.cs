using AutoMapper;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

internal class TeamMappingProfile : Profile
{
	public TeamMappingProfile()
	{
		CreateMap<Team, TeamItemDto>();
		CreateMap<Team, TeamDetailDto>();

		CreateMap<TeamMember, TeamMemberDto>()
			.ForMember(dest => dest.UserName, options => options.MapFrom(src => src.User.UserName))
			.ForMember(dest => dest.NickName, options => options.MapFrom(src => src.User.NickName))
			.ForMember(dest => dest.Email, options => options.MapFrom(src => src.User.Email))
			.ForMember(dest => dest.Phone, options => options.MapFrom(src => src.User.Phone));
	}
}