using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <summary>
/// 用户管理Controller
/// </summary>
[Route("api/[controller]")]
[ApiController, ApiExplorerSettings(GroupName = "identity")]
[Authorize]
public class UserController : ControllerBase
{
	private readonly IUserApplicationService _service;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="service"></param>
	public UserController(IUserApplicationService service)
	{
		_service = service;
	}

	/// <summary>
	/// 查询用户列表
	/// </summary>
	/// <param name="criteria"></param>
	/// <param name="skip"></param>
	/// <param name="count"></param>
	/// <returns></returns>
	[HttpGet]
	[Produces(typeof(List<UserItemDto>))]
	public async Task<IActionResult> QueryAsync([FromQuery] UserCriteria criteria, int skip = Constants.Query.Skip, int count = Constants.Query.Count)
	{
		var result = await _service.QueryAsync(criteria, skip, count, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 查询用户数量
	/// </summary>
	/// <param name="criteria"></param>
	/// <returns></returns>
	[HttpGet("count")]
	[Produces(typeof(int))]
	public async Task<IActionResult> CountAsync([FromQuery] UserCriteria criteria)
	{
		var result = await _service.CountAsync(criteria, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 查询用户详情
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpGet("{id}")]
	[Produces(typeof(UserDetailDto))]
	public async Task<IActionResult> GetAsync(string id)
	{
		var result = await _service.GetAsync(id, HttpContext.RequestAborted);
		return Ok(result);
	}

	/// <summary>
	/// 新增用户
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<IActionResult> CreateAsync([FromBody] UserCreateDto data)
	{
		var result = await _service.CreateAsync(data, HttpContext.RequestAborted);
		Response.Headers.Append("Entry", result);
		return Ok();
	}

	/// <summary>
	/// 更新用户
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateAsync(string id, [FromBody] UserUpdateDto data)
	{
		await _service.UpdateAsync(id, data, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 删除用户
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteAsync(string id)
	{
		await _service.DeleteAsync(id, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 设置用户角色
	/// </summary>
	/// <param name="id"></param>
	/// <param name="roles"></param>
	/// <returns></returns>
	[HttpPut("{id}/role")]
	public async Task<IActionResult> SetRoleAsync(string id, [FromBody] List<string> roles)
	{
		await _service.SetRolesAsync(id, roles, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 重置指定用户密码
	/// </summary>
	/// <param name="id"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPut("{id}/password")]
	[Authorize(Roles = "SA")]
	public async Task<IActionResult> ResetPasswordAsync(string id, [FromBody] ResetPasswordRequestDto data)
	{
		await _service.ResetPasswordAsync(id, data.Password, HttpContext.RequestAborted);
		return Ok();
	}

	/// <summary>
	/// 修改当前密码
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	[HttpPut("password")]
	public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequestDto data)
	{
		await _service.ChangePasswordAsync(data.OldPassword, data.NewPassword, HttpContext.RequestAborted);
		return Ok();
	}
}