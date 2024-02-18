using System.Text.Json;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;
using StackExchange.Redis;

namespace Nerosoft.Starfish.UseCases;

internal interface IPushRedisUseCase : INonOutputUseCase<PushRedisInput>;

internal record PushRedisInput(string Id, ConfigurationPushRedisRequestDto Data) : IUseCaseInput;

internal sealed class PushRedisUseCase : IPushRedisUseCase
{
	private readonly IConfigurationRepository _configurationRepository;
	private readonly ITeamRepository _appInfoRepository;
	private readonly UserPrincipal _identity;

	public PushRedisUseCase(IConfigurationRepository configurationRepository, ITeamRepository appInfoRepository, UserPrincipal identity)
	{
		_configurationRepository = configurationRepository;
		_appInfoRepository = appInfoRepository;
		_identity = identity;
	}

	public async Task ExecuteAsync(PushRedisInput input, CancellationToken cancellationToken = default)
	{
		var permission = await _appInfoRepository.CheckPermissionAsync(input.Id, _identity.UserId, cancellationToken);

		if (permission != PermissionState.Edit)
		{
			throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
		}

		var configuration = await _configurationRepository.GetAsync(input.Id, false, [nameof(Configuration.Archive)], cancellationToken);

		if (configuration == null)
		{
			throw new ConfigurationNotFoundException(input.Id);
		}

		if (configuration.Archive == null)
		{
			throw new NotFoundException("Archive not found.");
		}

		if (string.IsNullOrWhiteSpace(configuration.Archive.Data))
		{
			throw new InvalidDataException();
		}

		var data = GzipHelper.DecompressFromBase64(configuration.Archive.Data);

		var items = JsonSerializer.Deserialize<Dictionary<string, string>>(data);

		var connection = await ConnectionMultiplexer.ConnectAsync(input.Data.ConnectionString);
		using (connection)
		{
			var entries = items.Select(t => new HashEntry(t.Key, t.Value)).ToArray();

			var database = connection.GetDatabase(input.Data.Database);

			await database.HashSetAsync(input.Data.Key, entries);
			await database.KeyExpireAsync(input.Data.Key, default(TimeSpan?));
			await connection.CloseAsync();
		}
	}
}