using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置信息
/// </summary>
public class SettingNode : Aggregate<long>,
                           IHasCreateTime,
                           IHasUpdateTime
{
	private readonly SettingNodeType[] _sealedTypes =
	{
		SettingNodeType.String,
		SettingNodeType.Number,
		SettingNodeType.Boolean,
		SettingNodeType.Referer
	};

	private SettingNode()
	{
	}

	/// <summary>
	/// 父节点Id
	/// </summary>
	/// <remarks>
	/// 0表示根节点
	/// </remarks>
	public long ParentId { get; set; }

	/// <summary>
	/// 应用Id
	/// </summary>
	public long AppId { get; set; }

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
	public virtual SettingNode Parent { get; set; }

	/// <summary>
	/// 配置节点子节点
	/// </summary>
	public virtual HashSet<SettingNode> Children { get; set; }

	/// <summary>
	/// 配置节点所属应用
	/// </summary>
	public virtual AppInfo App { get; set; }

	/// <summary>
	/// 配置节点创建时间
	/// </summary>
	public DateTime CreateTime { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public DateTime UpdateTime { get; set; }

	internal static SettingNode CreateRoot(long appId, string appCode, string environment)
	{
		var entity = new SettingNode
		{
			AppId = appId,
			AppCode = appCode,
			Environment = environment,
			Name = $"{appCode}-{environment}",
			Type = SettingNodeType.Root,
			Status = SettingNodeStatus.Pending
		};

		return entity;
	}

	internal void AddArrayNode(string name, out SettingNode entity)
	{
		CheckSealed();

		Children ??= [];

		entity = new SettingNode
		{
			AppId = AppId,
			AppCode = AppCode,
			Environment = Environment,
			ParentId = Id,
			Name = Type == SettingNodeType.Array ? null : name,
			Type = SettingNodeType.Array,
			Status = SettingNodeStatus.Pending,
			Sort = Children.Count + 1,
			Key = GenerateKey(name, Children.Count + 1)
		};

		Children.Add(entity);
	}

	internal void AddObjectNode(string name, out SettingNode entity)
	{
		CheckSealed();

		Children ??= [];

		entity = new SettingNode
		{
			AppId = AppId,
			AppCode = AppCode,
			Environment = Environment,
			ParentId = Id,
			Name = Type == SettingNodeType.Array ? null : name,
			Type = SettingNodeType.Object,
			Status = SettingNodeStatus.Pending,
			Sort = Children.Count + 1,
			Key = GenerateKey(name, Children.Count + 1)
		};

		Children.Add(entity);
	}

	internal void AddStringNode(string name, string value, out SettingNode entity)
	{
		CheckSealed();

		Children ??= [];

		entity = new SettingNode
		{
			AppId = AppId,
			AppCode = AppCode,
			Environment = Environment,
			ParentId = Id,
			Name = Type == SettingNodeType.Array ? null : name,
			Value = value,
			Type = SettingNodeType.String,
			Status = SettingNodeStatus.Pending,
			Sort = Children.Count + 1,
			Key = GenerateKey(name, Children.Count + 1)
		};

		Children.Add(entity);
	}

	internal void AddBooleanNode(string name, string value, out SettingNode entity)
	{
		CheckSealed();

		Children ??= [];

		if (!bool.TryParse(value, out var result))
		{
			throw new BadRequestException("配置值不是有效的布尔值");
		}

		entity = new SettingNode
		{
			AppId = AppId,
			AppCode = AppCode,
			Environment = Environment,
			ParentId = Id,
			Name = Type == SettingNodeType.Array ? null : name,
			Value = result.ToString(),
			Type = SettingNodeType.Boolean,
			Status = SettingNodeStatus.Pending,
			Sort = Children.Count + 1,
			Key = GenerateKey(name, Children.Count + 1)
		};

		Children.Add(entity);
	}

	internal void AddNumberNode(string name, string value, out SettingNode entity)
	{
		CheckSealed();

		Children ??= [];

		if (!value.IsDecimal())
		{
			throw new BadRequestException("配置值不是有效的数字");
		}

		entity = new SettingNode
		{
			AppId = AppId,
			AppCode = AppCode,
			Environment = Environment,
			ParentId = Id,
			Name = Type == SettingNodeType.Array ? null : name,
			Value = value,
			Type = SettingNodeType.Number,
			Status = SettingNodeStatus.Pending,
			Sort = Children.Count + 1,
			Key = GenerateKey(name, Children.Count + 1)
		};

		Children.Add(entity);
	}

	internal void SetName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return;
		}

		if (Type == SettingNodeType.Array)
		{
			throw new InvalidOperationException("数组节点不允许修改名称");
		}

		Name = name;
	}

	private void CheckSealed()
	{
		if (_sealedTypes.Contains(Type))
		{
			throw new InvalidOperationException("该节点类型不允许添加子节点");
		}
	}

	internal string GenerateKey(string name, int sort)
	{
		return Type switch
		{
			SettingNodeType.Root => name,
			SettingNodeType.Array => $"{Key}:{sort - 1}",
			SettingNodeType.Object => $"{Key}:{name}",
			_ => string.Empty
		};
	}
}