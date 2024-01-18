using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

public class SettingRevisionBusiness : CommandObject<SettingRevisionBusiness>, IDomainService
{
	private readonly ISettingRepository _repository;

	public SettingRevisionBusiness(ISettingRepository repository)
	{
		_repository = repository;
	}

	[FactoryExecute]
	protected async Task ExecuteAsync(long appId, string environment, SettingRevisionArgument argument, CancellationToken cancellationToken = default)
	{
		var aggregate = await _repository.GetAsync(appId, environment, true, [nameof(Setting.Items), nameof(Setting.Revisions)], cancellationToken);

		if (aggregate == null)
		{
			throw new SettingNotFoundException(appId, environment);
		}

		aggregate.CreateRevision(argument.Version, argument.Comment, argument.Username);
		await _repository.UpdateAsync(aggregate, true, cancellationToken);
	}
}

public record SettingRevisionArgument(string Version, string Comment, string Username);