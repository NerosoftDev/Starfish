using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 设置聚合根
/// </summary>
public class Setting : Aggregate<long>, IAuditing
{
	private Setting()
	{
		Register<SettingStatusChangedEvent>(@event =>
		{
			Status = @event.NewStatus;
		});
	}

	/// <summary>
	/// 应用Id
	/// </summary>
	public long AppId { get; set; }

	/// <summary>
	/// 应用编码
	/// </summary>
	public string AppCode { get; set; }

	/// <summary>
	/// 环境名称
	/// </summary>
	public string Environment { get; set; }

	/// <summary>
	/// 描述
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// 状态
	/// </summary>
	public SettingStatus Status { get; set; }

	public DateTime CreateTime { get; set; }

	public DateTime UpdateTime { get; set; }

	/// <summary>
	/// 创建人账号
	/// </summary>
	public string CreatedBy { get; set; }

	/// <summary>
	/// 更新人账号
	/// </summary>
	public string UpdatedBy { get; set; }

	/// <summary>
	/// 设置项
	/// </summary>
	public HashSet<SettingNode> Nodes { get; set; }

	/// <summary>
	/// 历史版本
	/// </summary>
	public HashSet<SettingRevision> Revisions { get; set; }

	/// <summary>
	/// 应用信息
	/// </summary>
	public AppInfo App { get; set; }

	internal static Setting Create(long appId, string appCode, string environment, string description, IDictionary<string, string> nodes)
	{
		var setting = new Setting
		{
			AppId = appId,
			AppCode = appCode,
			Environment = environment,
			Status = SettingStatus.Pending,
			Description = description,
			Nodes = []
		};

		foreach (var (key, value) in nodes)
		{
			setting.AddNode(key, value);
		}

		setting.RaiseEvent(new SettingCreatedEvent(setting));
		return setting;
	}

	internal void AddNode(string key, string value)
	{
		Nodes ??= [];
		if (Nodes.Any(x => x.Key == key))
		{
			throw new ConflictException(string.Format(Resources.IDS_ERROR_SETTING_NODE_KEY_EXISTS, key));
		}

		var node = new SettingNode(key, value);
		Nodes.Add(node);
	}

	internal void SetStatus(SettingStatus status)
	{
		if (Status == status)
		{
			return;
		}

		RaiseEvent(new SettingStatusChangedEvent(Status, status));
	}

	internal void SetDescription(string description)
	{
		if (string.Equals(Description, description))
		{
			return;
		}

		Description = description;
	}
}