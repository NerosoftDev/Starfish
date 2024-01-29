using System.Text.Json;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;
using StackExchange.Redis;

namespace Nerosoft.Starfish.UseCases;

internal interface IPushRedisUseCase : INonOutputUseCase<PushRedisInput>;

internal record PushRedisInput(string AppId, string Environment, PushRedisRequestDto Data) : IUseCaseInput;

internal sealed class PushRedisUseCase : IPushRedisUseCase
{
	private readonly IConfigurationArchiveRepository _configurationRepository;
	private readonly IAppInfoRepository _appInfoRepository;
	private readonly UserPrincipal _identity;

	public PushRedisUseCase(IConfigurationArchiveRepository configurationRepository, IAppInfoRepository appInfoRepository, UserPrincipal identity)
	{
		_configurationRepository = configurationRepository;
		_appInfoRepository = appInfoRepository;
		_identity = identity;
	}

	public async Task ExecuteAsync(PushRedisInput input, CancellationToken cancellationToken = default)
	{
		var permission = await _appInfoRepository.CheckPermissionAsync(input.AppId, _identity.UserId, cancellationToken);

		if (!permission.IsIn(1, 2))
		{
			throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
		}

		var archive = await _configurationRepository.GetAsync(input.AppId, input.Environment, cancellationToken);

		if (archive == null)
		{
			throw new ConfigurationNotFoundException(input.AppId, input.Environment);
		}

		if (string.IsNullOrWhiteSpace(archive.Data))
		{
			throw new InvalidDataException();
		}

		var data = GzipHelper.DecompressFromBase64(archive.Data);

		var items = JsonSerializer.Deserialize<Dictionary<string, string>>(data);

		var connection = ConnectionMultiplexer.Connect(input.Data.ConnectionString);
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