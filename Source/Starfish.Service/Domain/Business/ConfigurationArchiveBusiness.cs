using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Common;
using Newtonsoft.Json;

namespace Nerosoft.Starfish.Domain;

public class ConfigurationArchiveBusiness : CommandObject<ConfigurationArchiveBusiness>, IDomainService
{
	[Inject]
	public IConfigurationRepository ConfigurationRepository { get; set; }

	[Inject]
	public IConfigurationArchiveRepository ArchiveRepository { get; set; }

	[FactoryExecute]
	protected async Task ExecuteAsync(long appId, string environment, string userName, CancellationToken cancellationToken = default)
	{
		var aggregate = await ConfigurationRepository.GetAsync(appId, environment, false, [nameof(Configuration.Items)], cancellationToken);

		if (aggregate == null)
		{
			throw new ConfigurationNotFoundException(appId, environment);
		}

		var data = aggregate.Items.ToDictionary(t => t.Key, t => t.Value);
		var json = JsonConvert.SerializeObject(data);

		var archive = await ArchiveRepository.GetAsync(appId, environment, cancellationToken);

		archive ??= ConfigurationArchive.Create(appId, environment);

		archive.Update(GzipHelper.CompressToBase64(json), userName);

		if (archive.Id > 0)
		{
			await ArchiveRepository.UpdateAsync(archive, true, cancellationToken);
		}
		else
		{
			await ArchiveRepository.InsertAsync(archive, true, cancellationToken);
		}
	}
}