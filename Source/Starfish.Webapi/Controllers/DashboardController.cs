using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// 客户端
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
	private readonly ConnectionContainer _container;
	private readonly IConfigurationApplicationService _configService;
	private readonly ITeamApplicationService _teamService;

	/// <inheritdoc />
	public DashboardController(ConnectionContainer container, IConfigurationApplicationService configService, ITeamApplicationService teamService)
	{
		_container = container;
		_configService = configService;
		_teamService = teamService;
	}

	/// <summary>
	/// 获取配置数量
	/// </summary>
	/// <returns></returns>
	[HttpGet("configurations/count")]
	public async Task<IActionResult> GetConfigurationCountAsync()
	{
		var count = await _configService.CountAsync(new ConfigurationCriteria(), HttpContext.RequestAborted);
		return Ok(count);
	}

	/// <summary>
	/// 获取配置项数量
	/// </summary>
	/// <returns></returns>
	[HttpGet("configurations/items/count")]
	public async Task<IActionResult> GetConfigurationItemCountAsync()
	{
		var count = await _configService.GetItemCountAsync(string.Empty, string.Empty, HttpContext.RequestAborted);
		return Ok(count);
	}

	/// <summary>
	/// 获取当前连接数量
	/// </summary>
	/// <returns></returns>
	[HttpGet("connections/count")]
	public async Task<IActionResult> GetClientCountAsync()
	{
		var count = await Task.Run(() => _container.GetConnections().Count);
		return Ok(count);
	}

	/// <summary>
	/// 获取连接列表
	/// </summary>
	/// <returns></returns>
	[HttpGet("connections")]
	public async Task<IActionResult> GetConnectionsAsync()
	{
		var connections = await Task.Run(() =>
		{
			return _container.GetConnections()
			                 .OrderByDescending(t => t.ConnectedTime)
			                 .Take(10);
		});
		return Ok(connections);
	}

	/// <summary>
	/// 获取团队数量
	/// </summary>
	/// <returns></returns>
	[HttpGet("teams/count")]
	public async Task<IActionResult> GetTeamCountAsync()
	{
		var count = await _teamService.CountAsync(new TeamCriteria(), HttpContext.RequestAborted);
		return Ok(count);
	}
}