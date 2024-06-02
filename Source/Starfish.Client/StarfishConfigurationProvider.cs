using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Nerosoft.Starfish.Common;

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
		_cacheFile = Path.Combine(_options.CacheDirectory, $"{_options.Id}.starfish.cache");
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
		if (string.IsNullOrWhiteSpace(args.Host))
		{
			return;
		}

		var uri = new Uri(args.Host);
		IConfigurationClient client = uri.Scheme switch
		{
			"http" or "https" => new HttpConfigurationClient(uri, _options.Id, _options.Secret),
			"ws" or "wss" => new SocketConfigurationClient(uri, _options.Id, _options.Secret),
			_ => throw new NotSupportedException(string.Format(Resources.IDS_ERROR_SCHEMA_NOT_SUPPORTED, uri.Scheme)),
		};
		try
		{
			await client.GetConfigurationAsync((data, length) =>
			{
				var json = GzipHelper.Decompress(data, length);
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