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
	public string AppId { get; set; }

	/// <summary>
	/// 密钥
	/// </summary>
	public string AppSecret { get; set; }

	/// <summary>
	/// 环境名称
	/// </summary>
	public string Environment { get; set; }

	/// <summary>
	/// 配置中心主机地址
	/// </summary>
	/// <remarks>
	/// <para>多个主机地址使用半角逗号或分号分割</para>
	/// <para> http://localhost:5000, ws://localhost:5000, https://localhost:5000, wss://localhost:5000, localhost:5000</para>
	/// <para>支持的协议类型包括http,https,ws,wss</para>
	/// </remarks>
	public string Host { get; set; }

	/// <summary>
	/// 配置缓存目录
	/// </summary>
	public string CacheDirectory { get; set; }

	/// <summary>
	/// 从Json文件加载配置
	/// </summary>
	/// <param name="jsonSettingsFile"></param>
	/// <returns></returns>
	public static ConfigurationClientOptions LoadJson(string jsonSettingsFile = "appsettings.json")
	{
		ArgumentNullException.ThrowIfNull(jsonSettingsFile);

		var configuration = new ConfigurationBuilder().AddJsonFile(jsonSettingsFile, true)
													  .AddEnvironmentVariables()
													  .Build();

		return Load(configuration);
	}

	/// <summary>
	/// 从Xml文件加载配置
	/// </summary>
	/// <param name="xmlSettingsFile"></param>
	/// <returns></returns>
	public static ConfigurationClientOptions LoadXml(string xmlSettingsFile = "config.xml")
	{
		ArgumentNullException.ThrowIfNull(xmlSettingsFile);

		var configuration = new ConfigurationBuilder().AddXmlFile(xmlSettingsFile, true)
													  .AddEnvironmentVariables()
													  .Build();

		return Load(configuration);
	}

	/// <summary>
	/// 从字典加载配置
	/// </summary>
	/// <param name="dictionary"></param>
	/// <returns></returns>
	public static ConfigurationClientOptions LoadDictionary(Dictionary<string, string> dictionary)
	{
		ArgumentNullException.ThrowIfNull(dictionary);

		var configuration = new ConfigurationBuilder().AddInMemoryCollection(dictionary)
													  .AddEnvironmentVariables()
													  .Build();

		return Load(configuration);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="configuration"></param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public static ConfigurationClientOptions Load(IConfiguration configuration)
	{
		var section = configuration.GetSection("Starfish");

		if (!section.Exists())
		{
			throw new InvalidOperationException("Starfish section not found in configuration");
		}

		var appId = section[nameof(AppId)];

		if (string.IsNullOrWhiteSpace(appId))
		{
			throw new InvalidOperationException("AppId not found in configuration");
		}

		var host = section[nameof(Host)];
		if (string.IsNullOrWhiteSpace(host))
		{
			throw new InvalidOperationException("Host not found in configuration");
		}

		var options = new ConfigurationClientOptions
		{
			AppId = appId,
			AppSecret = section[nameof(AppSecret)],
			Environment = section[nameof(Environment)] ?? System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
			Host = host,
			CacheDirectory = section[nameof(CacheDirectory)] ?? AppDomain.CurrentDomain.BaseDirectory
		};

		return options;
	}
}