using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// 字典接口
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = "system")]
[AllowAnonymous]
public class DictionaryController : ControllerBase
{
	private readonly IDictionaryApplicationService _service;

	/// <summary>
	/// 初始化字典Controller
	/// </summary>
	/// <param name="service"></param>
	public DictionaryController(IDictionaryApplicationService service)
	{
		_service = service;
	}

	/// <summary>
	/// 获取角色列表
	/// </summary>
	/// <returns></returns>
	[HttpGet("role")]
	public async Task<IActionResult> GetRoleItemsAsync()
	{
		var result = await _service.GetRoleItemsAsync();
		return Ok(result);
	}

	/// <summary>
	/// 获取环境列表
	/// </summary>
	/// <returns></returns>
	[HttpGet("environment")]
	public async Task<IActionResult> GetEnvironmentItemsAsync()
	{
		var result = await _service.GetEnvironmentItemsAsync();
		return Ok(result);
	}

	/// <summary>
	/// 获取支持的数据库类型列表
	/// </summary>
	/// <returns></returns>
	[HttpGet("database-type")]
	public async Task<IActionResult> GetDatabaseTypeItemsAsync()
	{
		var result = await _service.GetDatabaseTypeItemsAsync();
		return Ok(result);
	}

	/// <summary>
	/// 获取支持的配置节点类型列表
	/// </summary>
	/// <returns></returns>
	[HttpGet("setting-node-type")]
	public async Task<IActionResult> GetSettingNodeTypeItemsAsync()
	{
		var result = await _service.GetSettingNodeTypeItemsAsync();
		return Ok(result);
	}
}