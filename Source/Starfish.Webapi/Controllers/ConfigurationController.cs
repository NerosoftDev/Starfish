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
[ApiController, ApiExplorerSettings(GroupName = "configuration")]
[Authorize]
public class ConfigurationController : ControllerBase
{
	private readonly IConfigurationApplicationService _service;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="service"></param>
	public ConfigurationController(IConfigurationApplicationService service)
	{
		_service = service;
	}

	/// <summary>
	/// 查询配置列表
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="skip"></param>
	/// <param name="count"></param>
	/// <returns></returns>
	[HttpGet]
	[Produces<List<ConfigurationDto>>]
	public async Task<IActionResult> QueryAsync([FromQuery] ConfigurationCriteria criteria, int skip = Constants.Query.Skip, int count = Constants.Query.Count)
	{
		var result = await _service.QueryAsync(criteria, skip, count, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 查询配置数量
	/// </summary>
	/// <param name="criteria"></param>
	/// <returns></returns>
	[HttpGet("count")]
	[Produces(typeof(int))]
	public async Task<IActionResult> CountAsync([FromQuery] ConfigurationCriteria criteria)
	{
		var result = await _service.CountAsync(criteria, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 获取配置节点详情
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <returns></returns>
	[HttpGet("{id}")]
	[Produces<ConfigurationDto>]
	public async Task<IActionResult> GetAsync(string id)
	{
		var result = await _service.GetDetailAsync(id, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 新增配置
	/// </summary>
	/// <param name="teamId">团队Id</param>
	/// <param name="data">配置基本信息</param>
	/// <returns></returns>
	[HttpPost]
	public async Task<IActionResult> CreateAsync(string teamId, [FromBody] ConfigurationEditDto data)
	{
		var result = await _service.CreateAsync(teamId, data, HttpContext.RequestAborted);
		Response.Headers.Append("Entry", $"{result}");
		return Ok();
	}

	/// <summary>
	/// 更新配置
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="data">配置基本信息</param>
	/// <returns></returns>
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateAsync(string id, [FromBody] ConfigurationEditDto data)
	{
		await _service.UpdateAsync(id, data, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 删除配置
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <returns></returns>
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteAsync(string id)
	{
		await _service.DeleteAsync(id, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 设置访问密钥
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPut("{id}/secret")]
	public async Task<IActionResult> SetSecretAsync(string id, [FromBody] ConfigurationSecretSetRequestDto data)
	{
		await _service.SetSecretAsync(id, data.Secret, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 禁用配置
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpPut("{id}/disable")]
	public async Task<IActionResult> DisableAsync(string id)
	{
		await _service.DisableAsync(id, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 启用配置
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpPut("{id}/enable")]
	public async Task<IActionResult> EnableAsync(string id)
	{
		await _service.EnableAsync(id, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 发布配置
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost("{id}/publish")]
	public async Task<IActionResult> PublishAsync(string id, [FromBody] ConfigurationPublishRequestDto data)
	{
		await _service.PublishAsync(id, data, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 推送到Redis
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost("{id}/redis")]
	public async Task<IActionResult> PushRedisAsync(string id, [FromBody] ConfigurationPushRedisRequestDto data)
	{
		await _service.PushRedisAsync(id, data, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 获取配置项列表
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="skip"></param>
	/// <param name="count"></param>
	/// <param name="format"></param>
	/// <returns></returns>
	[HttpGet("{id}/item")]
	[Produces(typeof(List<ConfigurationItemDto>))]
	public async Task<IActionResult> GetItemListAsync(string id, int skip = Constants.Query.Skip, int count = Constants.Query.Count, [FromHeader(Name = "x-format")] string format = null)
	{
		switch (format)
		{
			case Constants.Configuration.FormatText:
				var text = await _service.GetItemsInTextAsync(id, "text", HttpContext.RequestAborted);
				return Ok(text);
			case Constants.Configuration.FormatJson:
				var json = await _service.GetItemsInTextAsync(id, "json", HttpContext.RequestAborted);
				return Ok(json);
			default:
				var result = await _service.GetItemListAsync(id, skip, count, HttpContext.RequestAborted);
				return Ok(result);
		}
	}

	/// <summary>
	/// 获取配置项数量
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <returns></returns>
	[HttpGet("{id}/item/count")]
	[Produces(typeof(int))]
	public async Task<IActionResult> GetItemCountAsync(string id)
	{
		var result = await _service.GetItemCountAsync(id, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 更新配置项的值
	/// </summary>
	/// <param name="id">应用Id</param>
	/// <param name="key">完整Key名称</param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPut("{id}/item/{key}")]
	[Consumes("text/plain")]
	public async Task<IActionResult> UpdateValueAsync(string id, string key, [FromBody] string data)
	{
		key = HttpUtility.UrlDecode(key);
		await _service.UpdateValueAsync(id, key, data, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 更新配置项
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPut("{id}/item")]
	public async Task<IActionResult> UpdateItemsAsync(string id, [FromBody] ConfigurationItemsUpdateDto data)
	{
		await _service.UpdateItemsAsync(id, data, HttpContext.RequestAborted);
		return Ok();
	}
}