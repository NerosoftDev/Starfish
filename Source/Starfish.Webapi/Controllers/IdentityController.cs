using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// 认证授权Controller
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = "identity")]
[AllowAnonymous]
public class IdentityController : ControllerBase
{
	private readonly IIdentityApplicationService _service;

	/// <summary>
	/// 初始化<see cref="IdentityController"/>
	/// </summary>
	/// <param name="service"></param>
	public IdentityController(IIdentityApplicationService service)
	{
		_service = service;
	}

	/// <summary>
	/// 获取Token
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
	[HttpPost("grant")]
	public async Task<IActionResult> GrantTokenAsync([FromBody] AuthRequestDto request)
	{
		var data = new Dictionary<string, string>
		{
			{ "username", request.UserName },
			{ "password", request.Password },
			{ "grant_type", "password" }
		};
		var result = await _service.GrantAsync("password", data, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 刷新Token
	/// </summary>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPost("refresh")]
	public async Task<IActionResult> RefreshTokenAsync(string token)
	{
		var data = new Dictionary<string, string>
		{
			{ "refresh_token", token },
			{ "grant_type", "refresh_token" }
		};
		var result = await _service.GrantAsync("refresh_token", data, HttpContext.RequestAborted);
		return Ok(result);
	}
}