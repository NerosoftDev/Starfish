using System.Text.RegularExpressions;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Service;
using Newtonsoft.Json;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 设置聚合根
/// </summary>
public sealed class Configuration : Aggregate<string>, IAuditing
{
	private Configuration()
	{
		Register<ConfigurationStatusChangedEvent>(@event =>
		{
			Status = @event.NewStatus;
		});
		Register<ConfigurationNameChangedEvent>(@event =>
		{
			Name = @event.NewName;
		});
		Register<ConfigurationSecretChangedEvent>(@event =>
		{
			Secret = @event.Secret;
		});
		Register<ConfigurationPublishedEvent>(_ =>
		{
			SetStatus(ConfigurationStatus.Published);
		});
		Register<ConfigurationItemChangedEvent>(_ =>
		{
			SetStatus(ConfigurationStatus.Pending);
		});
	}

	private Configuration(string teamId, string name)
		: this()
	{
		TeamId = teamId;
		Name = name;
		Status = ConfigurationStatus.Pending;
	}

	/// <summary>
	/// 团队Id
	/// </summary>
	public string TeamId { get; set; }

	/// <summary>
	/// 配置名称
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// 配置描述
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// 访问密钥
	/// </summary>
	public string Secret { get; set; }

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
	/// 配置归档
	/// </summary>
	public ConfigurationArchive Archive { get; set; }

	internal static Configuration Create(string teamId, string name)
	{
		var configuration = new Configuration(teamId, name);
		configuration.RaiseEvent(new ConfigurationCreatedEvent(configuration));
		return configuration;
	}

	internal void SetName(string name)
	{
		EnsureStatus();

		ArgumentException.ThrowIfNullOrWhiteSpace(name);
		if (string.Equals(name, Name, StringComparison.OrdinalIgnoreCase))
		{
			return;
		}

		RaiseEvent(new ConfigurationNameChangedEvent(Name, name));
	}

	private void SetStatus(ConfigurationStatus status)
	{
		if (Status == status)
		{
			return;
		}

		RaiseEvent(new ConfigurationStatusChangedEvent(Status, status));
	}

	/// <summary>
	/// 设置访问密钥
	/// </summary>
	/// <param name="secret"></param>
	/// <exception cref="BadRequestException">密钥不符合规则时引发异常</exception>
	internal void SetSecret(string secret)
	{
		EnsureStatus();

		ArgumentException.ThrowIfNullOrWhiteSpace(secret);

		if (!Regex.IsMatch(secret, Constants.RegexPattern.Secret))
		{
			throw new BadRequestException(Resources.IDS_ERROR_CONFIG_SECRET_NOT_MATCHES_RULE);
		}

		if (!string.IsNullOrEmpty(Id))
		{
			RaiseEvent(new ConfigurationSecretChangedEvent(Cryptography.SHA.Encrypt(secret)));
		}
	}

	internal void UpdateItem(IDictionary<string, string> items)
	{
		EnsureStatus();

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

		RaiseEvent(new ConfigurationItemChangedEvent());
	}

	internal void UpdateItem(string key, string value)
	{
		EnsureStatus();

		Items ??= [];

		var item = Items.FirstOrDefault(t => string.Equals(t.Key, key));

		if (item == null)
		{
			throw new InvalidOperationException(string.Format(Resources.IDS_ERROR_CONFIG_KEY_NOT_EXISTS, key));
		}

		if (string.Equals(item.Value, value))
		{
			return;
		}

		item.Value = value;

		RaiseEvent(new ConfigurationItemChangedEvent());
	}

	private void CreateRevision(string version, string comment, string @operator)
	{
		Revisions ??= [];

		if (Revisions.Any(t => string.Equals(t.Version, version, StringComparison.OrdinalIgnoreCase)))
		{
			throw new InvalidOperationException(string.Format(Resources.IDS_ERROR_CONFIG_VERSION_NUMBER_EXISTS, version));
		}

		var data = JsonConvert.SerializeObject(Items, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

		var revision = new ConfigurationRevision(version, comment, data, @operator);

		Revisions.Add(revision);
	}

	private void CreateArchive(string @operator)
	{
		Archive ??= ConfigurationArchive.Create(Id);
		var data = Items.ToDictionary(t => t.Key, t => t.Value);

		Archive.Update(JsonConvert.SerializeObject(data), @operator);
	}

	internal void Publish(string version, string comment, string @operator)
	{
		EnsureStatus();

		if (Items == null || Items.Count == 0)
		{
			throw new InvalidOperationException("配置项为空");
		}

		CreateArchive(@operator);
		CreateRevision(version, comment, @operator);
		Version = version;
		PublishTime = DateTime.Now;
		RaiseEvent(new ConfigurationPublishedEvent());
	}

	private void EnsureStatus()
	{
		if (Status == ConfigurationStatus.Disabled)
		{
			throw new ConfigurationDisabledException(Id);
		}
	}

	internal void Disable()
	{
		if (Status == ConfigurationStatus.Disabled)
		{
			return;
		}

		SetStatus(ConfigurationStatus.Disabled);
	}

	internal void Enable()
	{
		if (Status != ConfigurationStatus.Disabled)
		{
			return;
		}

		SetStatus(ConfigurationStatus.Pending);
	}

	internal void SetDescription(string description)
	{
		Description = description;
	}
}