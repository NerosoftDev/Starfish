using System.Text.Json;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;
using StackExchange.Redis;

namespace Nerosoft.Starfish.UseCases;

internal interface IConfigurationPushRedisUseCase : INonOutputUseCase<ConfigurationPushRedisInput>;

internal record ConfigurationPushRedisInput(string Id, ConfigurationPushRedisRequestDto Data) : IUseCaseInput;

internal sealed class ConfigurationPushRedisUseCase : IConfigurationPushRedisUseCase
{
	private readonly IConfigurationRepository _configurationRepository;
	private readonly ITeamRepository _teamRepository;
	private readonly UserPrincipal _identity;

	public ConfigurationPushRedisUseCase(IConfigurationRepository configurationRepository, ITeamRepository teamRepository, UserPrincipal identity)
	{
		_configurationRepository = configurationRepository;
		_teamRepository = teamRepository;
		_identity = identity;
	}

	public async Task ExecuteAsync(ConfigurationPushRedisInput input, CancellationToken cancellationToken = default)
	{
		var permission = await _teamRepository.CheckPermissionAsync(input.Id, _identity.UserId, cancellationToken);

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