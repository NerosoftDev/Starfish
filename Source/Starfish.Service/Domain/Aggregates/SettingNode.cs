using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置信息
/// </summary>
public class SettingNode : Aggregate<long>, IHasCreateTime, IHasUpdateTime
{
	private SettingNode()
	{ }

	/// <summary>
	/// 父节点Id
	/// </summary>
	/// <remarks>
	/// 0表示根节点
	/// </remarks>
	public long ParentId { get; set; }

	/// <summary>
	/// 应用唯一编码
	/// </summary>
	public string AppCode { get; set; }

	/// <summary>
	/// 环境名称
	/// </summary>
	public string Environment { get; set; }

	/// <summary>
	/// 配置名称
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// 配置值
	/// </summary>
	public string Value { get; set; }

	/// <summary>
	/// 节点唯一键
	/// </summary>
	public string Key { get; set; }

	/// <summary>
	/// 配置描述
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// 配置节点类型
	/// </summary>
	public SettingNodeType Type { get; set; }

	/// <summary>
	/// 配置节点状态
	/// </summary>
	public SettingNodeStatus Status { get; set; }

	/// <summary>
	/// 配置节点排序
	/// </summary>
	public int Sort { get; set; }

	/// <summary>
	/// 配置节点父节点
	/// </summary>
	public SettingNode Parent { get; set; }

	/// <summary>
	/// 配置节点子节点
	/// </summary>
	public HashSet<SettingNode> Children { get; set; }

	/// <summary>
	/// 配置节点创建时间
	/// </summary>
	public DateTime CreateTime { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public DateTime UpdateTime { get; set; }

	internal static SettingNode CreateRoot(string appCode, string enviroment)
	{
		var entity = new SettingNode
		{
			AppCode = appCode,
			Environment = enviroment,
			Type = SettingNodeType.Root,
			Status = SettingNodeStatus.Pending
		};

		return entity;
	}

	internal void AddArrayNode(string name)
	{
		Children ??= [];

		var entity = new SettingNode
		{
			AppCode = AppCode,
			Environment = Environment,
			ParentId = Id,
			Name = name,
			Type = SettingNodeType.Array,
			Status = SettingNodeStatus.Pending,
			Sort = Children.Count + 1,
			Key = GenerateKey(name, Children.Count + 1)
		};

		Children.Add(entity);
	}

	internal void AddObjectNode(string name)
	{
		Children ??= [];

		var entity = new SettingNode
		{
			AppCode = AppCode,
			Environment = Environment,
			ParentId = Id,
			Name = name,
			Type = SettingNodeType.Object,
			Status = SettingNodeStatus.Pending,
			Sort = Children.Count + 1,
			Key = GenerateKey(name, Children.Count + 1)
		};

		Children.Add(entity);
	}

	internal void AddStringNode(string name, string value)
	{
		Children ??= [];

		var entity = new SettingNode
		{
			AppCode = AppCode,
			Environment = Environment,
			ParentId = Id,
			Name = name,
			Value = value,
			Type = SettingNodeType.String,
			Status = SettingNodeStatus.Pending,
			Sort = Children.Count + 1,
			Key = GenerateKey(name, Children.Count + 1)
		};

		Children.Add(entity);
	}

	internal void AddBooleanNode(string name, bool value)
	{
		Children ??= [];

		var entity = new SettingNode
		{
			AppCode = AppCode,
			Environment = Environment,
			ParentId = Id,
			Name = name,
			Value = value.ToString(),
			Type = SettingNodeType.Boolean,
			Status = SettingNodeStatus.Pending,
			Sort = Children.Count + 1,
			Key = GenerateKey(name, Children.Count + 1)
		};

		Children.Add(entity);
	}

	internal void AddNumberNode(string name, string value)
	{
		Children ??= [];

		var entity = new SettingNode
		{
			AppCode = AppCode,
			Environment = Environment,
			ParentId = Id,
			Name = name,
			Value = value,
			Type = SettingNodeType.Number,
			Status = SettingNodeStatus.Pending,
			Sort = Children.Count + 1,
			Key = GenerateKey(name, Children.Count + 1)
		};

		Children.Add(entity);
	}

	private string GenerateKey(string name, int sort)
	{
		return Type switch
		{
			SettingNodeType.Root => name,
			SettingNodeType.Array => $"{Key}:{sort - 1}",
			SettingNodeType.Object => $"{Key}:{Name}",
			_ => string.Empty
		};
	}
}