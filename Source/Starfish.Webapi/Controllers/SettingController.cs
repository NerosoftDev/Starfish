using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// 应用配置管理接口
/// </summary>
[Route("api/[controller]")]
[ApiController]
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
}