using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置节点重命名领域事件
/// </summary>
public class SettingNodeRenamedEvent : DomainEvent
{
	/// <summary>
	/// 
	/// </summary>
	public SettingNodeRenamedEvent()
	{
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="oldName"></param>
	/// <param name="newName"></param>
	public SettingNodeRenamedEvent(long id, string oldName, string newName)
	{
		Id = id;
		OldName = oldName;
		NewName = newName;
	}

	/// <summary>
	/// 节点Id
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	/// 旧名称
	/// </summary>
	public string OldName { get; set; }

	/// <summary>
	/// 新名称
	/// </summary>
	public string NewName { get; set; }
}