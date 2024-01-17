using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Service;
using Newtonsoft.Json;

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
	/// 应用环境
	/// </summary>
	public string Environment { get; set; }

	/// <summary>
	/// 状态
	/// </summary>
	public SettingStatus Status { get; set; }

	/// <summary>
	/// 当前版本号
	/// </summary>
	public string Version { get; set; }

	/// <summary>
	/// 发布时间
	/// </summary>
	public DateTime? PublishTime { get; set; }

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
	public HashSet<SettingItem> Items { get; set; }

	/// <summary>
	/// 历史版本
	/// </summary>
	public HashSet<SettingRevision> Revisions { get; set; }

	/// <summary>
	/// 应用信息
	/// </summary>
	public AppInfo App { get; set; }

	internal static Setting Create(long appId, string appCode, string environment, IDictionary<string, string> items)
	{
		var setting = new Setting
		{
			AppId = appId,
			AppCode = appCode,
			Environment = environment,
			Status = SettingStatus.Pending
		};

		setting.AddOrUpdateItem(items);

		setting.RaiseEvent(new SettingCreatedEvent(setting));
		return setting;
	}

	internal void AddOrUpdateItem(IDictionary<string, string> items)
	{
		if (Status == SettingStatus.Disabled)
		{
			throw new InvalidOperationException(Resources.IDS_ERROR_SETTING_DISABLED);
		}

		Items ??= [];
		Items.RemoveAll(t => !items.ContainsKey(t.Key));
		foreach (var (key, value) in items)
		{
			var item = Items.FirstOrDefault(t => t.Key == key);
			if (item == null)
			{
				item = new SettingItem(key, value);
				Items.Add(item);
			}
			else
			{
				item.Value = value;
			}
		}

		Status = SettingStatus.Pending;
	}

	internal void UpdateItem(string key, string value)
	{
		if (Status == SettingStatus.Disabled)
		{
			throw new InvalidOperationException(Resources.IDS_ERROR_SETTING_DISABLED);
		}

		Items ??= [];

		var item = Items.FirstOrDefault(t => string.Equals(t.Key, key));

		if (item == null)
		{
			throw new InvalidOperationException(string.Format(Resources.IDS_ERROR_SETTING_KEY_NOT_EXISTS, key));
		}

		item.Value = value;

		Status = SettingStatus.Pending;
	}

	internal void SetStatus(SettingStatus status)
	{
		if (Status == status)
		{
			return;
		}

		RaiseEvent(new SettingStatusChangedEvent(Status, status));
	}

	internal void CreateRevision(string version, string comment, string @operator)
	{
		if (Items == null || Items.Count == 0)
		{
			return;
		}

		Revisions ??= [];

		if (Revisions.Any(t => string.Equals(t.Version, version, StringComparison.OrdinalIgnoreCase)))
		{
			throw new InvalidOperationException(string.Format(Resources.IDS_ERROR_SETTING_VERSION_NUMBER_EXISTS, version));
		}

		var data = JsonConvert.SerializeObject(Items, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

		var revision = new SettingRevision(version, comment, data, @operator);

		Revisions.Add(revision);

		Version = version;
		PublishTime = DateTime.Now;
	}

	internal void Archive(string @operator)
	{
	}
}