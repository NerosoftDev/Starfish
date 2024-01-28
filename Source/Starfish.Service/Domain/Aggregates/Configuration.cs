using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Service;
using Newtonsoft.Json;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 设置聚合根
/// </summary>
public class Configuration : Aggregate<long>, IAuditing
{
	private Configuration()
	{
		Register<ConfigurationStatusChangedEvent>(@event =>
		{
			Status = @event.NewStatus;
		});
	}

	/// <summary>
	/// 应用Id
	/// </summary>
	public long AppId { get; set; }

	/// <summary>
	/// 应用环境
	/// </summary>
	public string Environment { get; set; }

	/// <summary>
	/// 状态
	/// </summary>
	public ConfigurationStatus Status { get; set; }

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
	public HashSet<ConfigurationItem> Items { get; set; }

	/// <summary>
	/// 历史版本
	/// </summary>
	public HashSet<ConfigurationRevision> Revisions { get; set; }

	/// <summary>
	/// 应用信息
	/// </summary>
	public AppInfo App { get; set; }

	internal static Configuration Create(long appId, string environment, IDictionary<string, string> items)
	{
		var setting = new Configuration
		{
			AppId = appId,
			Environment = environment,
			Status = ConfigurationStatus.Pending
		};

		setting.AddOrUpdateItem(items);

		setting.RaiseEvent(new ConfigurationCreatedEvent(setting));
		return setting;
	}

	internal void AddOrUpdateItem(IDictionary<string, string> items)
	{
		if (Status == ConfigurationStatus.Disabled)
		{
			throw new InvalidOperationException(Resources.IDS_ERROR_CONFIG_DISABLED);
		}

		Items ??= [];
		Items.RemoveAll(t => !items.ContainsKey(t.Key));
		foreach (var (key, value) in items)
		{
			var item = Items.FirstOrDefault(t => t.Key == key);
			if (item == null)
			{
				item = new ConfigurationItem(key, value);
				Items.Add(item);
			}
			else
			{
				item.Value = value;
			}
		}

		Status = ConfigurationStatus.Pending;
	}

	internal void UpdateItem(string key, string value)
	{
		if (Status == ConfigurationStatus.Disabled)
		{
			throw new InvalidOperationException(Resources.IDS_ERROR_CONFIG_DISABLED);
		}

		Items ??= [];

		var item = Items.FirstOrDefault(t => string.Equals(t.Key, key));

		if (item == null)
		{
			throw new InvalidOperationException(string.Format(Resources.IDS_ERROR_CONFIG_KEY_NOT_EXISTS, key));
		}

		item.Value = value;

		Status = ConfigurationStatus.Pending;
	}

	internal void SetStatus(ConfigurationStatus status)
	{
		if (Status == status)
		{
			return;
		}

		RaiseEvent(new ConfigurationStatusChangedEvent(Status, status));
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
			throw new InvalidOperationException(string.Format(Resources.IDS_ERROR_CONFIG_VERSION_NUMBER_EXISTS, version));
		}

		var data = JsonConvert.SerializeObject(Items, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

		var revision = new ConfigurationRevision(version, comment, data, @operator);

		Revisions.Add(revision);

		Version = version;
		PublishTime = DateTime.Now;
	}
}