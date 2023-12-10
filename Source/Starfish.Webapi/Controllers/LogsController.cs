using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// 日志管理接口
/// </summary>
[Route("api/[controller]")]
[ApiController]
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
	/// 搜索日志
	/// </summary>
	/// <param name="criteria">查询条件</param>
	/// <param name="page">页码</param>
	/// <param name="size">数量</param>
	/// <returns>符合条件的日志列表</returns>
	[HttpGet]
	public async Task<IActionResult> SearchAsync([FromQuery] OperateLogCriteria criteria, int page, int size = 10)
	{
		var result = await _service.SearchAsync(criteria, page, size);
		return Ok(result);
	}
}