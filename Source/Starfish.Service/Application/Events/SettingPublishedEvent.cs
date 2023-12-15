using Nerosoft.Euonia.Domain;

// ReSharper disable MemberCanBePrivate.Global

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置节点发布完成事件
/// </summary>
public class SettingPublishedEvent : ApplicationEvent
{
	public SettingPublishedEvent()
	{
	}

	public SettingPublishedEvent(long id)
		: this()
	{
		Id = id;
	}

	/// <summary>
	/// 根节点Id
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	/// 版本号
	/// </summary>
	public string Version { get; set; }

	/// <summary>
	/// 描述
	/// </summary>
	public string Comment { get; set; }
}