using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// 应用配置管理接口
/// </summary>
[Route("api/setting/node")]
[ApiController, ApiExplorerSettings(GroupName = "setting")]
[Authorize]
public class SettingNodeController : ControllerBase
{
	private readonly ISettingApplicationService _service;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="service"></param>
	public SettingNodeController(ISettingApplicationService service)
	{
		_service = service;
	}

	/// <summary>
	/// 查询符合条件的配置节点列表
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="page"></param>
	/// <param name="size"></param>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> SearchAsync([FromQuery] SettingNodeCriteria criteria, int page = 1, int size = 10)
	{
		var result = await _service.SearchAsync(criteria, page, size, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 获取符合条件的配置节点数量
	/// </summary>
	/// <param name="criteria"></param>
	/// <returns></returns>
	[HttpGet("count")]
	public async Task<IActionResult> CountAsync([FromQuery] SettingNodeCriteria criteria)
	{
		var result = await _service.CountAsync(criteria, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 获取配置节点详情
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpGet("{id:long}")]
	public async Task<IActionResult> GetAsync(long id)
	{
		var result = await _service.GetAsync(id, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 新增根节点
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<IActionResult> CreateRootAsync([FromBody] SettingNodeCreateDto data)
	{
		var result = await _service.CreateRootNodeAsync(data.AppId, data.Environment, HttpContext.RequestAborted);
		Response.Headers.Append("Entry", $"{result}");
		return Ok();
	}

	/// <summary>
	/// 新增子节点
	/// </summary>
	/// <param name="id">父节点Id</param>
	/// <param name="type"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost("{id:long}/{type}")]
	public async Task<IActionResult> CreateLeafAsync(long id, SettingNodeType type, [FromBody] SettingNodeCreateDto data)
	{
		var result = await _service.CreateLeafNodeAsync(id, type, data, HttpContext.RequestAborted);
		Response.Headers.Append("Entry", $"{result}");
		return Ok();
	}

	/// <summary>
	/// 更新配置节点
	/// </summary>
	/// <param name="id">节点Id</param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPut("{id:long}/value")]
	public async Task<IActionResult> UpdateValueAsync(long id, [FromBody] SettingNodeUpdateDto data)
	{
		await _service.UpdateValueAsync(id, data.Value, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 重命名配置节点
	/// </summary>
	/// <param name="id">节点Id</param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPut("{id:long}/name")]
	public async Task<IActionResult> UpdateNameAsync(long id, [FromBody] SettingNodeUpdateDto data)
	{
		await _service.UpdateNameAsync(id, data.Name, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 设置配置节点描述
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPut("{id:long}/description")]
	public async Task<IActionResult> UpdateDescriptionAsync(long id, [FromBody] SettingNodeUpdateDto data)
	{
		await _service.UpdateDescriptionAsync(id, data.Description, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 删除配置节点
	/// </summary>
	/// <param name="id">节点Id</param>
	/// <returns></returns>
	[HttpDelete("{id:long}")]
	public async Task<IActionResult> DeleteAsync(long id)
	{
		await _service.DeleteAsync(id, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 发布配置
	/// </summary>
	/// <param name="id">根节点Id</param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost("{id:long}/publish")]
	public async Task<IActionResult> PublishAsync(long id, [FromBody] SettingNodePublishDto data)
	{
		await _service.PublishAsync(id, data, HttpContext.RequestAborted);
		return Ok();
	}
}