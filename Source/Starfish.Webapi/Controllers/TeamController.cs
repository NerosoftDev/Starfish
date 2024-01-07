using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// 团队管理Controller
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = "identity")]
[Authorize]
public class TeamController : ControllerBase
{
	private readonly ITeamApplicationService _service;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="service"></param>
	public TeamController(ITeamApplicationService service)
	{
		_service = service;
	}

	/// <summary>
	/// 搜索团队
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="page"></param>
	/// <param name="size"></param>
	/// <returns></returns>
	[HttpGet]
	[Produces(typeof(List<TeamItemDto>))]
	public async Task<IActionResult> QueryAsync([FromQuery] TeamCriteria criteria, int page = 1, int size = 10)
	{
		var result = await _service.QueryAsync(criteria, page, size, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 获取团队数量
	/// </summary>
	/// <param name="criteria"></param>
	/// <returns></returns>
	[HttpGet("count")]
	[Produces(typeof(int))]
	public async Task<IActionResult> CountAsync([FromQuery] TeamCriteria criteria)
	{
		var result = await _service.CountAsync(criteria, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 创建团队
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<IActionResult> CreateAsync([FromBody] TeamEditDto data)
	{
		var result = await _service.CreateAsync(data, HttpContext.RequestAborted);
		Response.Headers.Append("Entry", $"{result}");
		return Ok();
	}

	/// <summary>
	/// 更新团队
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPut("{id:int}")]
	public async Task<IActionResult> UpdateAsync(int id, [FromBody] TeamEditDto data)
	{
		await _service.UpdateAsync(id, data, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 查询团队成员
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpGet("{id:int}/member")]
	[Produces(typeof(List<TeamMemberDto>))]
	public async Task<IActionResult> QueryMembersAsync(int id)
	{
		var result = await _service.QueryMembersAsync(id, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 添加团队成员
	/// </summary>
	/// <param name="id"></param>
	/// <param name="userIds"></param>
	/// <returns></returns>
	[HttpPost("{id:int}/member")]
	public async Task<IActionResult> AppendMembersAsync(int id, [FromBody] List<int> userIds)
	{
		await _service.AppendMembersAsync(id, userIds, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 移除团队成员
	/// </summary>
	/// <param name="id"></param>
	/// <param name="userIds"></param>
	/// <returns></returns>
	[HttpDelete("{id:int}/member")]
	public async Task<IActionResult> RemoveMembersAsync(int id, [FromBody] List<int> userIds)
	{
		await _service.RemoveMembersAsync(id, userIds, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 退出团队
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpDelete("{id:int}/quit")]
	public async Task<IActionResult> QuitAsync(int id)
	{
		await _service.QuitAsync(id, HttpContext.RequestAborted);
		return Ok();
	}
}