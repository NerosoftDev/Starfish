using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 用户认证成功事件
/// </summary>
public class UserAuthSucceedEvent : ApplicationEvent
{
	/// <summary>
	/// 初始化<see cref="UserAuthSucceedEvent"/>实例。
	/// </summary>
	public UserAuthSucceedEvent()
	{
	}

	/// <summary>
	/// 初始化<see cref="UserAuthSucceedEvent"/>实例。
	/// </summary>
	/// <param name="authType"></param>
	/// <param name="data"></param>
	/// <param name="userId"></param>
	public UserAuthSucceedEvent(string authType, Dictionary<string, string> data, int userId)
	{
		AuthType = authType;
		Data = data;
		UserId = userId;
	}

	/// <summary>
	/// 认证类型
	/// </summary>
	public string AuthType { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public Dictionary<string, string> Data { get; set; }

	/// <summary>
	/// 用户Id
	/// </summary>
	public int UserId { get; set; }

	/// <summary>
	/// 用户名
	/// </summary>
	public string UserName { get; set; }

	/// <summary>
	/// Refresh token.
	/// </summary>
	public string RefreshToken { get; set; }

	/// <summary>
	/// 令牌颁发时间
	/// </summary>
	public DateTime TokenIssueTime { get; set; }
}