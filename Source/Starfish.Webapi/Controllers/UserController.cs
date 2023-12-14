using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// 用户管理Controller
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = "identity")]
public class UserController : ControllerBase
{
}