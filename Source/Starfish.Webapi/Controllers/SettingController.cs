using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// 应用配置管理接口
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = "setting")]
[Authorize]
public class SettingController : ControllerBase
{
	private readonly ISettingApplicationService _service;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="service"></param>
	public SettingController(ISettingApplicationService service)
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
	public async Task<IActionResult> SearchAsync([FromQuery] SettingCriteria criteria, int page = 1, int size = 10)
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
	public async Task<IActionResult> CountAsync([FromQuery] SettingCriteria criteria)
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
	public async Task<IActionResult> CreateAsync([FromBody] SettingCreateDto data)
	{
		var result = await _service.CreateAsync(data, HttpContext.RequestAborted);
		Response.Headers.Append("Entry", $"{result}");
		return Ok();
	}

	/// <summary>
	/// 更新配置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPut("{id:long}")]
	public async Task<IActionResult> UpdateAsync(long id, [FromBody] SettingUpdateDto data)
	{
		await _service.UpdateAsync(id, data, HttpContext.RequestAborted);
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
	/// 更新配置项的值
	/// </summary>
	/// <param name="id">配置Id</param>
	/// <param name="key">完整Key名称</param>
	/// <param name="value"></param>
	/// <returns></returns>
	[HttpPut("{id:long}/{key}")]
	[Consumes("plain/text")]
	public async Task<IActionResult> UpdateItemAsync(long id, string key, [FromBody] string value)
	{
		key = HttpUtility.UrlDecode(key);
		await _service.UpdateAsync(id, key, value, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 发布配置
	/// </summary>
	/// <param name="id">根节点Id</param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost("{id:long}/publish")]
	public async Task<IActionResult> PublishAsync(long id, [FromBody] SettingPublishDto data)
	{
		await _service.PublishAsync(id, data, HttpContext.RequestAborted);
		return Ok();
	}
}