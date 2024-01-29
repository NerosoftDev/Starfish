using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

public class ConfigurationRevisionBusiness : CommandObject<ConfigurationRevisionBusiness>, IDomainService
{
	private readonly IConfigurationRepository _repository;

	public ConfigurationRevisionBusiness(IConfigurationRepository repository)
	{
		_repository = repository;
	}

	[FactoryExecute]
	protected async Task ExecuteAsync(string appId, string environment, ConfigurationRevisionArgument argument, CancellationToken cancellationToken = default)
	{
		var aggregate = await _repository.GetAsync(appId, environment, true, [nameof(Configuration.Items), nameof(Configuration.Revisions)], cancellationToken);

		if (aggregate == null)
		{
			throw new ConfigurationNotFoundException(appId, environment);
		}

		aggregate.CreateRevision(argument.Version, argument.Comment, argument.Username);
		await _repository.UpdateAsync(aggregate, true, cancellationToken);
	}
}

public record ConfigurationRevisionArgument(string Version, string Comment, string Username);