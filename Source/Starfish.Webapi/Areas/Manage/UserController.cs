using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Nerosoft.Starfish.Webapi.Areas.Manage;

/// <summary>
/// 用户管理Controller
/// </summary>
[Route("api/[area]/[controller]")]
[ApiController]
[Area("manage")]
public class UserController : ControllerBase
{
}
