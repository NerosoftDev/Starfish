using Nerosoft.Euonia.Domain;

// ReSharper disable MemberCanBePrivate.Global

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置节点发布完成事件
/// </summary>
public class ConfigurationPublishedEvent : ApplicationEvent
{
	public ConfigurationPublishedEvent()
	{
	}

	public ConfigurationPublishedEvent(long appId, string environment)
		: this()
	{
		AppId = appId;
		Environment = environment;
	}

	/// <summary>
	/// 根节点Id
	/// </summary>
	public long AppId { get; set; }

	/// <summary>
	/// 应用环境
	/// </summary>
	public string Environment { get; set; }

	/// <summary>
	/// 版本号
	/// </summary>
	public string Version { get; set; }

	/// <summary>
	/// 描述
	/// </summary>
	public string Comment { get; set; }
}