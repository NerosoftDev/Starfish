using System.IO.Compression;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Nerosoft.Starfish.Client;

internal class StarfishConfigurationProvider : ConfigurationProvider, IDisposable
{
	public event EventHandler<HostChangedEventArgs> HostChanged;
	public event EventHandler ConnectionLost;

	private readonly ConfigurationClientOptions _options;

	private readonly string _cacheFile;

	private readonly CountdownEvent _waitHandle = new(1);

	private readonly IEnumerator<string> _hosts;
	private readonly char[] _separator = [',', ';'];

	public StarfishConfigurationProvider(ConfigurationClientOptions options)
	{
		_options = options;
		_cacheFile = Path.Combine(_options.CacheDirectory, $"{_options.App}.starfish.{_options.Env}.cache");
		HostChanged += OnHostChanged;
		ConnectionLost += (_, _) =>
		{
			if (_hosts != null && _hosts.MoveNext())
			{
				HostChanged?.Invoke(this, new HostChangedEventArgs(_options.Host, CancellationToken.None));
			}
		};

		_hosts = _options.Host
		                 .Split(_separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
		                 .Distinct()
		                 .ToList()
		                 .GetEnumerator();
		_hosts.MoveNext();
		HostChanged?.Invoke(this, new HostChangedEventArgs(_hosts.Current, CancellationToken.None));
	}

	public override void Load()
	{
		_waitHandle.Wait(TimeSpan.FromSeconds(30));

		if (!File.Exists(_cacheFile))
		{
			return;
		}

		var data = File.ReadAllText(_cacheFile);
		Data = JsonSerializer.Deserialize<Dictionary<string, string>>(data);
	}

	private async void OnHostChanged(object sender, HostChangedEventArgs args)
	{
		var uri = new Uri(args.Host);
		IConfigurationClient client = uri.Scheme switch
		{
			"http" or "https" => new HttpConfigurationClient(uri, _options.Team, _options.App, _options.Secret, _options.Env),
			"ws" or "wss" => new SocketConfigurationClient(uri, _options.Team, _options.App, _options.Secret, _options.Env),
			_ => throw new NotSupportedException(string.Format(Resources.IDS_ERROR_SCHEMA_NOT_SUPPORTED, uri.Scheme)),
		};
		try
		{
			await client.GetConfigurationAsync((data, length) =>
			{
				var json = Decompress(data, length);
				File.WriteAllText(_cacheFile, json, Encoding.UTF8);
				if (_waitHandle.IsSet)
				{
					Load();
					OnReload();
				}
				else
				{
					_waitHandle.Signal();
				}
			}, args.CancellationToken);
		}
		catch (Exception)
		{
			ConnectionLost?.Invoke(this, EventArgs.Empty);
		}
	}

	private static string Decompress(byte[] data, int count)
	{
		var stream = new MemoryStream(data, 0, count);
		var zip = new GZipStream(stream, CompressionMode.Decompress, true);
		var destStream = new MemoryStream();
		var buffer = new byte[0x1000];
		while (true)
		{
			var reader = zip.Read(buffer, 0, buffer.Length);
			if (reader <= 0)
			{
				break;
			}

			destStream.Write(buffer, 0, reader);
		}

		zip.Close();
		stream.Close();
		destStream.Position = 0;
		buffer = destStream.ToArray();
		destStream.Close();
		return Encoding.UTF8.GetString(buffer);
	}

	public void Dispose()
	{
		_waitHandle.Dispose();
		HostChanged -= OnHostChanged;
		GC.SuppressFinalize(this);
	}

	public class HostChangedEventArgs : EventArgs
	{
		public HostChangedEventArgs(string host, CancellationToken cancellationToken)
		{
			Host = host;
			CancellationToken = cancellationToken;
		}

		public string Host { get; }

		public CancellationToken CancellationToken { get; }
	}
}