﻿using Nerosoft.Euonia.Domain;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置信息
/// </summary>
public class SettingNode : Aggregate<long>,
                           IHasCreateTime,
                           IHasUpdateTime
{
	private readonly SettingNodeType[] _sealedTypes =
	[
		SettingNodeType.String,
		SettingNodeType.Number,
		SettingNodeType.Boolean,
		SettingNodeType.Referer
	];

	private SettingNode()
	{
	}

	/// <summary>
	/// 根节点Id
	/// </summary>
	public long RootId { get; set; }

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
			Type = SettingNodeType.Root
		};
		entity.RaiseEvent(new SettingNodeCreatedEvent(entity));
		return entity;
	}

	internal void AddArrayNode(string name, out SettingNode entity)
	{
		CheckSealed();

		Children ??= [];

		entity = new SettingNode
		{
			RootId = Type == SettingNodeType.Root ? Id : RootId,
			AppId = AppId,
			AppCode = AppCode,
			Environment = Environment,
			ParentId = Id,
			Type = SettingNodeType.Array,
			Sort = Children.Count + 1,
			Key = GenerateKey(name, Children.Count + 1)
		};

		if (Type != SettingNodeType.Array)
		{
			CheckName(name);
			entity.Name = name;
		}

		Children.Add(entity);

		RaiseEvent(new SettingNodeCreatedEvent(entity));
	}

	internal void AddObjectNode(string name, out SettingNode entity)
	{
		CheckSealed();

		Children ??= [];

		entity = new SettingNode
		{
			RootId = Type == SettingNodeType.Root ? Id : RootId,
			AppId = AppId,
			AppCode = AppCode,
			Environment = Environment,
			ParentId = Id,
			Type = SettingNodeType.Object,
			Sort = Children.Count + 1,
			Key = GenerateKey(name, Children.Count + 1)
		};

		if (Type != SettingNodeType.Array)
		{
			CheckName(name);
			entity.Name = name;
		}

		Children.Add(entity);

		RaiseEvent(new SettingNodeCreatedEvent(entity));
	}

	internal void AddStringNode(string name, string value, out SettingNode entity)
	{
		CheckSealed();

		Children ??= [];

		entity = new SettingNode
		{
			RootId = Type == SettingNodeType.Root ? Id : RootId,
			AppId = AppId,
			AppCode = AppCode,
			Environment = Environment,
			ParentId = Id,
			Value = value,
			Type = SettingNodeType.String,
			Sort = Children.Count + 1,
			Key = GenerateKey(name, Children.Count + 1)
		};

		if (Type != SettingNodeType.Array)
		{
			CheckName(name);
			entity.Name = name;
		}

		Children.Add(entity);

		RaiseEvent(new SettingNodeCreatedEvent(entity));
	}

	internal void AddBooleanNode(string name, string value, out SettingNode entity)
	{
		CheckSealed();

		Children ??= [];

		if (!bool.TryParse(value, out var result))
		{
			throw new BadRequestException(Resources.IDS_ERROR_SETTING_NODE_VALUE_NOT_BOOLEAN);
		}

		entity = new SettingNode
		{
			RootId = Type == SettingNodeType.Root ? Id : RootId,
			AppId = AppId,
			AppCode = AppCode,
			Environment = Environment,
			ParentId = Id,
			Value = result.ToString(),
			Type = SettingNodeType.Boolean,
			Sort = Children.Count + 1,
			Key = GenerateKey(name, Children.Count + 1)
		};

		if (Type != SettingNodeType.Array)
		{
			CheckName(name);
			entity.Name = name;
		}

		Children.Add(entity);

		RaiseEvent(new SettingNodeCreatedEvent(entity));
	}

	internal void AddNumberNode(string name, string value, out SettingNode entity)
	{
		CheckSealed();

		Children ??= [];

		if (!value.IsDecimal())
		{
			throw new BadRequestException(Resources.IDS_ERROR_SETTING_NODE_VALUE_NOT_NUMBER);
		}

		entity = new SettingNode
		{
			RootId = Type == SettingNodeType.Root ? Id : RootId,
			AppId = AppId,
			AppCode = AppCode,
			Environment = Environment,
			ParentId = Id,
			Value = value,
			Type = SettingNodeType.Number,
			Sort = Children.Count + 1,
			Key = GenerateKey(name, Children.Count + 1)
		};

		if (Type != SettingNodeType.Array)
		{
			CheckName(name);
			entity.Name = name;
		}

		Children.Add(entity);

		RaiseEvent(new SettingNodeCreatedEvent(entity));
	}

	internal void SetName(string newName)
	{
		var oldName = Name;
		Name = newName;
		RaiseEvent(new SettingNodeRenamedEvent(Id, oldName, newName));
	}

	internal void SetKey(string key)
	{
		if (string.Equals(Key, key))
		{
			return;
		}

		Key = key;
	}

	internal void SetValue(string value)
	{
		if (!_sealedTypes.Contains(Type))
		{
			throw new BadRequestException(string.Format(Resources.IDS_ERROR_SETTING_NODE_NOT_ALLOW_SET_VALUE, Type));
		}

		if (string.Equals(Value, value))
		{
			return;
		}

		if (!string.IsNullOrWhiteSpace(value))
		{
			switch (Type)
			{
				case SettingNodeType.Number:
					if (!value.IsDecimal())
					{
						throw new BadRequestException(Resources.IDS_ERROR_SETTING_NODE_VALUE_NOT_NUMBER);
					}

					Value = value;
					break;
				case SettingNodeType.Boolean:
					if (!bool.TryParse(value, out var booleanValue))
					{
						throw new BadRequestException(Resources.IDS_ERROR_SETTING_NODE_VALUE_NOT_BOOLEAN);
					}

					value = booleanValue.ToString();
					break;
			}
		}

		Value = value;
	}

	internal void SetDescription(string description)
	{
		if (string.Equals(Description, description))
		{
			return;
		}

		Description = description;
	}

	private void CheckName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new BadRequestException(Resources.IDS_ERROR_SETTING_NODE_NAME_REQUIRED);
		}

		if (Children.Any(t => string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase)))
		{
			throw new ConflictException(Resources.IDS_ERROR_SETTING_NODE_NAME_EXISTS);
		}
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