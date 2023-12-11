using AutoMapper;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 日志映射配置
/// </summary>
public class LogsMappingProfile : Profile
{
	/// <summary>
	/// 初始化日志映射配置
	/// </summary>
	public LogsMappingProfile()
	{
		CreateMap<OperateLog, OperateLogDto>()
			.ForMember(dest => dest.Type, opt => opt.MapFrom(src => GetTypeName(src.Type)));
	}

	private static string GetTypeName(string type)
	{
		return type.ToLowerInvariant() switch
		{
			"auth.password" => "密码登录",
			"auth.refresh_token" => "刷新Token",
			"auth.otp" => "验证码登录",
			_ => type
		};
	}
}