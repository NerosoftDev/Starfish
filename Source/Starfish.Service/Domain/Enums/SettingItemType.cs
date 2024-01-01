namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置节点类型
/// </summary>
public enum SettingItemType
{
	/// <summary>
	/// 根节点
	/// </summary>
	Root,

	/// <summary>
	/// 数组
	/// </summary>
	Array,

	/// <summary>
	/// 对象
	/// </summary>
	Object,

	/// <summary>
	/// 字符串
	/// </summary>
	String,

	/// <summary>
	/// 数字
	/// </summary>
	Number,

	/// <summary>
	/// 布尔
	/// </summary>
	Boolean,

	/// <summary>
	/// 引用
	/// </summary>
	Referer
}