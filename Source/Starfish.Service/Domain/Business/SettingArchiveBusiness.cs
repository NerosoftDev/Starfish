using System.IO.Compression;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Newtonsoft.Json;

namespace Nerosoft.Starfish.Domain;

public class SettingArchiveBusiness : CommandObject<SettingArchiveBusiness>, IDomainService
{
	[Inject]
	public ISettingRepository SettingRepository { get; set; }

	[Inject]
	public ISettingArchiveRepository ArchiveRepository { get; set; }

	[FactoryExecute]
	protected async Task ExecuteAsync(long appId, string environment, string userName, CancellationToken cancellationToken = default)
	{
		var aggregate = await SettingRepository.GetAsync(appId, environment, false, [nameof(Setting.Items)], cancellationToken);

		if (aggregate == null)
		{
			throw new SettingNotFoundException(appId, environment);
		}

		var data = aggregate.Items.ToDictionary(t => t.Key, t => t.Value);
		var json = JsonConvert.SerializeObject(data);

		var archive = await ArchiveRepository.GetAsync(appId, environment, cancellationToken);

		archive ??= SettingArchive.Create(appId, environment);

		archive.Update(Compress(json), userName);

		if (archive.Id > 0)
		{
			await ArchiveRepository.UpdateAsync(archive, true, cancellationToken);
		}
		else
		{
			await ArchiveRepository.InsertAsync(archive, true, cancellationToken);
		}
	}

	private static string Compress(string source)
	{
		var data = Encoding.UTF8.GetBytes(source);

		var stream = new MemoryStream();
		var zip = new GZipStream(stream, CompressionMode.Compress, true);
		zip.Write(data, 0, data.Length);
		zip.Close();
		var buffer = new byte[stream.Length];
		stream.Position = 0;
		_ = stream.Read(buffer, 0, buffer.Length);
		stream.Close();

		return Convert.ToBase64String(buffer);
	}
}