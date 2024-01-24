using Microsoft.Extensions.Configuration;
using Nerosoft.Starfish.Application;

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
	/// <para>Item2-刷新令牌</para>
	/// <para>Item3-颁发时间</para>
	/// <para>Item4-过期时间</para>
	/// </returns>
	public Tuple<string, string, DateTime, DateTime> GenerateAccessToken(long userId, string userName, IEnumerable<string> roles = null)
	{
		var issueTime = DateTime.UtcNow;
		var expiresAt = issueTime.AddDays(1);

		var builder = TokenGenerator.Create(userId.ToString(), userName)
		                            .WithSigningKey(_configuration.GetValue<string>("JwtBearerOptions:TokenKey"))
		                            .WithIssuer(_configuration.GetValue<string>("JwtBearerOptions:TokenIssuer"))
		                            .AddRole(roles?.ToArray())
		                            .IssuedAt(issueTime);

		var accessToken = builder.Build();
		var refreshToken = Guid.NewGuid().ToString("N");
		return Tuple.Create(accessToken, refreshToken, issueTime, expiresAt);
	}
}