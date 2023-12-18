using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nerosoft.Starfish.Webapi.Controllers;

/// <inheritdoc />
[Route("{controller=Home}/{action=Index}")]
[ApiExplorerSettings(IgnoreApi = true)]
[AllowAnonymous]
public class HomeController : Controller
{
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public IActionResult Index()
	{
		var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
		return environment switch
		{
			"Development" => Redirect("/swagger"),
			_ => Content($"@ {DateTime.Today.Year} Nerosoft.")
		};
	}
}
