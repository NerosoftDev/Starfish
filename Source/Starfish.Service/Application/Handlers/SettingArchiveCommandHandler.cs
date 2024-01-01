using System.IO.Compression;
using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;
using Newtonsoft.Json;

namespace Nerosoft.Starfish.Application;

public class SettingArchiveCommandHandler : CommandHandlerBase,
                                            IHandler<SettingArchiveCreateCommand>
{
	public SettingArchiveCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory)
		: base(unitOfWork, factory)
	{
	}

	public Task HandleAsync(SettingArchiveCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var (appId, appCode, environment, nodes) = await GetNodesAsync(message.RootId, cancellationToken);
			var data = nodes.ToDictionary(t => t.Key, t => t.Value);
			var json = JsonConvert.SerializeObject(data);

			var userName = context?.User?.Identity?.Name;

			await SaveArchiveAsync(appId, appCode, environment, json, userName, cancellationToken);
		});
	}

	private async Task<Tuple<long, string, string, List<SettingItem>>> GetNodesAsync(long id, CancellationToken cancellationToken = default)
	{
		var repository = UnitOfWork.Current.GetService<ISettingRepository>();

		var aggregate = await repository.GetAsync(id, false, [nameof(Setting.Items)], cancellationToken);

		if (aggregate == null)
		{
			throw new SettingNotFoundException(id);
		}

		return Tuple.Create(aggregate.AppId, aggregate.AppCode, aggregate.Environment, aggregate.Items.ToList());
	}

	private async Task SaveArchiveAsync(long appId, string appCode, string environment, string data, string userName, CancellationToken cancellationToken = default)
	{
		var repository = UnitOfWork.Current.GetService<ISettingArchiveRepository>();

		var archive = await repository.GetAsync(appCode, environment, cancellationToken);

		archive ??= SettingArchive.Create(appId, appCode, environment);

		archive.Update(Compress(data), userName);

		if (archive.Id > 0)
		{
			await repository.UpdateAsync(archive, true, cancellationToken);
		}
		else
		{
			await repository.InsertAsync(archive, true, cancellationToken);
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