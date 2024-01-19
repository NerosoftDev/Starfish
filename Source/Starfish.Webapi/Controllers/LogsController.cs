using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// 日志管理接口
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = "system")]
[Authorize]
public class LogsController : ControllerBase
{
	private readonly ILogsApplicationService _service;

	/// <summary>
	/// 初始化日志管理Controller
	/// </summary>
	/// <param name="service"></param>
	public LogsController(ILogsApplicationService service)
	{
		_service = service;
	}

	/// <summary>
	/// 列表查询
	/// </summary>
	/// <param name="criteria">查询条件</param>
	/// <param name="skip">页码，起始值为1。</param>
	/// <param name="count">数量</param>
	/// <returns>符合条件的日志列表</returns>
	[HttpGet]
	[Produces<List<OperateLogDto>>]
	public async Task<IActionResult> QueryAsync([FromQuery] OperateLogCriteria criteria, int skip = Constants.Query.Skip, int count = Constants.Query.Count)
	{
		var result = await _service.QueryAsync(criteria, skip, count);
		return Ok(result);
	}

	/// <summary>
	/// 数量查询
	/// </summary>
	/// <param name="criteria">查询条件</param>
	/// <returns>符合条件的日志数量</returns>
	[HttpGet("count")]
	[Produces<int>]
	public async Task<IActionResult> CountAsync([FromQuery] OperateLogCriteria criteria)
	{
		var result = await _service.CountAsync(criteria);
		return Ok(result);
	}
}