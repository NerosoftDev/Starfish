using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 新增子节点命令
/// </summary>
public class SettingLeafNodeCreateCommand : Command
{
	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="id"></param>
	/// <param name="type"></param>
	/// <param name="data"></param>
	public SettingLeafNodeCreateCommand(long id, SettingNodeType type, SettingNodeCreateDto data)
	{
		Id = id;
		Type = type;
		Data = data;
	}

	/// <summary>
	/// 父节点Id
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	/// 节点类型
	/// </summary>
	public SettingNodeType Type { get; set; }

	/// <summary>
	/// 节点数据
	/// </summary>
	public SettingNodeCreateDto Data { get; set; }
}