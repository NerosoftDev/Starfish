using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Nerosoft.Starfish.Application;

internal class UserInitializeService : BackgroundService
{
	private readonly IUserApplicationService _service;
	private readonly ILogger<UserInitializeService> _logger;

	public UserInitializeService(IUserApplicationService service, ILoggerFactory logger)
	{
		_service = service;
		_logger = logger.CreateLogger<UserInitializeService>();
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		try
		{
			await _service.InitializeAsync(stoppingToken);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);
		}
	}
}
