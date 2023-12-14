using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 配置节点发布完成事件
/// </summary>
public class SettingPublishedEvent : ApplicationEvent
{
	/// <summary>
	/// 构造函数
	/// </summary>
	public SettingPublishedEvent()
	{
	}

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id"></param>
	public SettingPublishedEvent(long id)
	{
		Id = id;
	}

	/// <summary>
	/// 配置Id
	/// </summary>
	public long Id { get; set; }
	
	/// <summary>
	/// 版本号
	/// </summary>
	public string Version { get; set; }

	/// <summary>
	/// 描述
	/// </summary>
	public string Description { get; set; }
}