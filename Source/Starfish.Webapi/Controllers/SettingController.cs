using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// 应用配置管理接口
/// </summary>
[Route("api/apps/{id:long}/[controller]/{environment}")]
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
	/// 获取配置项列表
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="environment">应用环境</param>
	/// <param name="skip"></param>
	/// <param name="count"></param>
	/// <param name="format"></param>
	/// <returns></returns>
	[HttpGet("item")]
	[Produces(typeof(List<SettingItemDto>))]
	public async Task<IActionResult> GetItemListAsync(long id, string environment, int skip = Constants.Query.Skip, int count = Constants.Query.Count, [FromHeader(Name = "x-format")] string format = null)
	{
		switch (format)
		{
			case Constants.Setting.FormatText:
				var text = await _service.GetItemsInTextAsync(id, environment, "text", HttpContext.RequestAborted);
				return Ok(text);
			case Constants.Setting.FormatJson:
				var json = await _service.GetItemsInTextAsync(id, environment, "json", HttpContext.RequestAborted);
				return Ok(json);
			default:
				var result = await _service.GetItemListAsync(id, environment, skip, count, HttpContext.RequestAborted);
				return Ok(result);
		}
	}

	/// <summary>
	/// 获取配置项数量
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="environment">应用环境</param>
	/// <returns></returns>
	[HttpGet("item/count")]
	[Produces(typeof(int))]
	public async Task<IActionResult> GetItemCountAsync(long id, string environment)
	{
		var result = await _service.GetItemCountAsync(id, environment, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 获取Json格式配置
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="environment">应用环境</param>
	/// <returns></returns>
	[HttpGet("item/json")]
	[Produces(typeof(string))]
	public async Task<IActionResult> GetJsonAsync(long id, string environment)
	{
		var json = await _service.GetItemsInTextAsync(id, environment, "json", HttpContext.RequestAborted);
		return Ok(json);
	}

	/// <summary>
	/// 获取配置节点详情
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="environment">应用环境</param>
	/// <returns></returns>
	[HttpGet("detail")]
	[Produces<SettingDetailDto>]
	public async Task<IActionResult> GetAsync(long id, string environment)
	{
		var result = await _service.GetDetailAsync(id, environment, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 新增配置
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="environment">应用环境</param>
	/// <param name="format"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<IActionResult> CreateAsync(long id, string environment, [FromHeader(Name = "x-format")] string format, [FromBody] SettingEditDto data)
	{
		var result = await _service.CreateAsync(id, environment, format, data, HttpContext.RequestAborted);
		Response.Headers.Append("Entry", $"{result}");
		return Ok();
	}

	/// <summary>
	/// 更新配置
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="environment">应用环境</param>
	/// <param name="format">数据格式</param>
	/// <param name="data">数据内容</param>
	/// <returns></returns>
	[HttpPut]
	public async Task<IActionResult> UpdateAsync(long id, string environment, [FromHeader(Name = "x-format")] string format, [FromBody] SettingEditDto data)
	{
		await _service.UpdateAsync(id, environment, format, data, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 删除配置
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="environment">应用环境</param>
	/// <returns></returns>
	[HttpDelete]
	public async Task<IActionResult> DeleteAsync(long id, string environment)
	{
		await _service.DeleteAsync(id, environment, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 更新配置项的值
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="environment">应用环境</param>
	/// <param name="key">完整Key名称</param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPut("{key}")]
	public async Task<IActionResult> UpdateItemValueAsync(long id, string environment, string key, [FromBody] SettingValueUpdateDto data)
	{
		key = HttpUtility.UrlDecode(key);
		await _service.UpdateAsync(id, environment, key, data.Value, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 发布配置
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="environment">应用环境</param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost("publish")]
	public async Task<IActionResult> PublishAsync(long id, string environment, [FromBody] SettingPublishDto data)
	{
		await _service.PublishAsync(id, environment, data, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 获取发布的配置
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="environment">应用环境</param>
	/// <returns></returns>
	[HttpGet("archive")]
	[Produces<string>]
	public async Task<IActionResult> GetArchivedAsync(long id, string environment)
	{
		var result = await _service.GetSettingRawAsync(id, environment, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 推送到Redis
	/// </summary>
	/// <param name="id"></param>
	/// <param name="environment"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost("redis")]
	public async Task<IActionResult> PushRedisAsync(long id, string environment, [FromBody] PushRedisRequestDto data)
	{
		await _service.PushRedisAsync(id, environment, data, HttpContext.RequestAborted);
		return Ok();
	}
}