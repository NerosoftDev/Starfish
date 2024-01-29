using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// 应用管理接口
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = "configuration")]
[Authorize]
public class AppsController : ControllerBase
{
	private readonly IAppsApplicationService _service;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="service"></param>
	public AppsController(IAppsApplicationService service)
	{
		_service = service;
	}

	/// <summary>
	/// 搜索应用信息
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="skip"></param>
	/// <param name="count"></param>
	/// <returns></returns>
	[HttpGet]
	[Produces<List<AppInfoItemDto>>]
	public async Task<IActionResult> SearchAsync([FromQuery] AppInfoCriteria criteria, int skip = Constants.Query.Skip, int count = Constants.Query.Count)
	{
		var result = await _service.QueryAsync(criteria, skip, count, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 查询应用数量
	/// </summary>
	/// <param name="criteria"></param>
	/// <returns></returns>
	[HttpGet("count")]
	[Produces<int>]
	public async Task<IActionResult> CountAsync([FromQuery] AppInfoCriteria criteria)
	{
		var result = await _service.CountAsync(criteria, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 获取应用详情
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpGet("{id:long}")]
	[Produces<AppInfoDetailDto>]
	public async Task<IActionResult> GetAsync(string id)
	{
		var result = await _service.GetAsync(id, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 创建应用
	/// </summary>
	/// <param name="model"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<IActionResult> CreateAsync([FromBody] AppInfoCreateDto model)
	{
		var result = await _service.CreateAsync(model, HttpContext.RequestAborted);
		//HttpContext.Response.Headers.Add("Location", $"/api/apps/{result}");
		HttpContext.Response.Headers.Append("Entry", result);
		return Ok(result);
	}

	/// <summary>
	/// 更新应用
	/// </summary>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <returns></returns>
	[HttpPut("{id:long}")]
	public async Task<IActionResult> UpdateAsync(string id, [FromBody] AppInfoUpdateDto model)
	{
		await _service.UpdateAsync(id, model, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 启用应用
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpPut("{id:long}/enable")]
	public async Task<IActionResult> EnableAsync(string id)
	{
		await _service.EnableAsync(id, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 禁用应用
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpPut("{id:long}/disable")]
	public async Task<IActionResult> DisableAsync(string id)
	{
		await _service.DisableAsync(id, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 删除应用
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpDelete("{id:long}")]
	public async Task<IActionResult> DeleteAsync(string id)
	{
		await _service.DeleteAsync(id, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 设置应用密钥
	/// </summary>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <returns></returns>
	[HttpPut("{id:long}/secret")]
	public async Task<IActionResult> SetSecretAsync(string id, [FromBody] AppInfoSetSecretDto model)
	{
		await _service.SetSecretAsync(id, model.Secret, HttpContext.RequestAborted);
		return Ok();
	}
}