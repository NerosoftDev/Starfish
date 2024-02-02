using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// 管理员管理Controller
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = "identity")]
[Authorize]
public class AdministratorController : ControllerBase
{
	private readonly IAdministratorApplicationService _service;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="service"></param>
	public AdministratorController(IAdministratorApplicationService service)
	{
		_service = service;
	}

	/// <summary>
	/// 查询管理员列表
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="skip"></param>
	/// <param name="count"></param>
	/// <returns></returns>
	[HttpGet]
	[Produces(typeof(List<AdministratorItemDto>))]
	public async Task<IActionResult> QueryAsync([FromQuery] AdministratorCriteria criteria, int skip = Constants.Query.Skip, int count = Constants.Query.Count)
	{
		var result = await _service.QueryAsync(criteria, skip, count, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 查询管理员数量
	/// </summary>
	/// <param name="criteria"></param>
	/// <returns></returns>
	[HttpGet("count")]
	[Produces(typeof(int))]
	public async Task<IActionResult> CountAsync([FromQuery] AdministratorCriteria criteria)
	{
		var result = await _service.CountAsync(criteria, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 创建管理员
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<IActionResult> AssignAsync([FromBody] AdministratorAssignDto data)
	{
		await _service.AssignAsync(data, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 删除管理员
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
	[HttpDelete("{userId}")]
	public async Task<IActionResult> DeleteAsync(string userId)
	{
		await _service.DeleteAsync(userId, HttpContext.RequestAborted);
		return Ok();
	}
}