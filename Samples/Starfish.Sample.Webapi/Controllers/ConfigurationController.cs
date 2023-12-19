using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Nerosoft.Starfish.Sample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConfigurationController : ControllerBase
{
	private readonly IConfiguration _configuration;

	public ConfigurationController(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	[HttpGet("connection")]
	public Task<IActionResult> GetConnectionString()
	{ 
		var result = _configuration.GetConnectionString("Default");
		return Task.FromResult<IActionResult>(Ok(result));
	}
	
	[HttpGet("section/{key}")]
	public async Task<IActionResult> GetSection(string key)
	{
		var result = _configuration.GetSection(key);
		return await Task.FromResult<IActionResult>(Ok(result));
	}
}
