using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;

// ReSharper disable UnusedMember.Global

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 应用配置发布领域服务
/// </summary>
public class SettingPublishBusiness : CommandObject<SettingPublishBusiness>, IDomainService
{
	private readonly ISettingRepository _repository;

	public SettingPublishBusiness(ISettingRepository repository)
	{
		_repository = repository;
	}

	[FactoryExecute]
	protected async Task ExecuteAsync(long id, CancellationToken cancellationToken = default)
	{
		var aggregate = await _repository.GetAsync(id, true, Array.Empty<string>(), cancellationToken);

		if (aggregate == null)
		{
			throw new SettingNotFoundException(id);
		}

		if (aggregate == null)
		{
			throw new SettingNotFoundException(id);
		}

		if (aggregate.Status == SettingStatus.Disabled)
		{
			throw new SettingDisabledException(id);
		}

		aggregate.SetStatus(SettingStatus.Published);

		await _repository.UpdateAsync(aggregate, true, cancellationToken);
	}
}