using Microsoft.Extensions.Configuration;

namespace Nerosoft.Starfish.Client;

/// <summary>
/// 客户选配置选项
/// </summary>
public class ConfigurationClientOptions
{
	/// <summary>
	/// 应用Id
	/// </summary>
	public string App { get; set; }

	/// <summary>
	/// 应用环境
	/// </summary>
	public string Env { get; set; }

	/// <summary>
	/// 密钥
	/// </summary>
	public string Secret { get; set; }

	/// <summary>
	/// 配置中心主机地址
	/// </summary>
	/// <remarks>
	/// <para>多个主机地址使用半角逗号或分号分割</para>
	/// <para> http://localhost:5000, ws://localhost:5000, https://localhost:5000, wss://localhost:5000</para>
	/// <para>支持的协议类型包括http,https,ws,wss</para>
	/// </remarks>
	public string Host { get; set; }

	/// <summary>
	/// 配置缓存目录
	/// </summary>
	public string CacheDirectory { get; set; }

	/// <summary>
	/// 从字典加载配置
	/// </summary>
	/// <param name="dictionary"></param>
	/// <returns></returns>
	public static ConfigurationClientOptions Load(Dictionary<string, string> dictionary)
	{
		ArgumentNullException.ThrowIfNull(dictionary);

		var configuration = new ConfigurationBuilder().AddInMemoryCollection(dictionary)
		                                              .Build();

		return Load(configuration);
	}

	/// <summary>
	/// 从已有配置信息加载Starfish.Client配置
	/// </summary>
	/// <param name="configuration"></param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public static ConfigurationClientOptions Load(IConfiguration configuration)
	{
		var section = configuration.GetSection("Starfish");

		if (!section.Exists())
		{
			throw new InvalidOperationException(Resources.IDS_ERROR_STARFISH_SECTION_NOT_FOUND);
		}

		var app = section[nameof(App)];

		if (string.IsNullOrWhiteSpace(app))
		{
			throw new InvalidOperationException(Resources.IDS_ERROR_APP_SECTION_NOT_FOUND);
		}

		var host = section[nameof(Host)];
		if (string.IsNullOrWhiteSpace(host))
		{
			throw new InvalidOperationException(Resources.IDS_ERROR_HOST_SECTION_NOT_FOUND);
		}

		var options = new ConfigurationClientOptions
		{
			Host = host,
			App = app,
			Env = section[nameof(Env)] ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
			Secret = section[nameof(Secret)],
			CacheDirectory = section[nameof(CacheDirectory)] ?? AppDomain.CurrentDomain.BaseDirectory
		};

		return options;
	}
}