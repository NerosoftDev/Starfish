using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 用户认证公共组件
/// </summary>
public class IdentityCommonComponent
{
	private readonly IConfiguration _configuration;

	/// <summary>
	/// 初始化<see cref="IdentityCommonComponent"/>.
	/// </summary>
	/// <param name="configuration"></param>
	public IdentityCommonComponent(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	/// <summary>
	/// 生成访问令牌
	/// </summary>
	/// <param name="userId">用户Id</param>
	/// <param name="userName">用户名</param>
	/// <param name="roles">角色</param>
	/// <returns>
	///	<para>Item1-访问令牌</para>
	/// <para>Item2-颁发时间</para>
	/// <para>Item3-过期时间</para>
	/// </returns>
	public Tuple<string, DateTime, DateTime> GenerateAccessToken(int userId, string userName, IEnumerable<string> roles = null)
	{
		var issueTime = DateTime.UtcNow;
		var expiresAt = issueTime.AddDays(1);
		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenKey = _configuration.GetValue<string>("JwtBearerOptions:TokenKey");
		var key = Encoding.UTF8.GetBytes(tokenKey.ToSha256());
		var issuer = _configuration.GetValue<string>("JwtBearerOptions:TokenIssuer");
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new[]
			{
				new Claim(JwtClaimTypes.Issuer, issuer),
				new Claim(JwtClaimTypes.Subject, userId.ToString()),
				new Claim(JwtClaimTypes.Name, userName)
			}),
			Expires = expiresAt,
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
		if (roles?.Any() == true)
		{
			foreach (var role in roles)
			{
				tokenDescriptor.Subject.AddClaim(new Claim(JwtClaimTypes.Role, role));
			}
		}

		var token = tokenHandler.CreateToken(tokenDescriptor);
		var tokenString = tokenHandler.WriteToken(token);
		return Tuple.Create(tokenString, issueTime, expiresAt);
	}
}