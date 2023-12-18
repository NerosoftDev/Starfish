using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 应用信息
/// </summary>
public sealed class AppInfo : Aggregate<long>,
                              IHasCreateTime,
                              IHasUpdateTime
{
	/// <summary>
	/// 此构造方法仅提供给EntityFramework使用
	/// </summary>
	private AppInfo()
	{
	}

	/// <summary>
	/// 团队Id
	/// </summary>
	public long? TeamId { get; set; }

	/// <summary>
	/// 名称
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// 唯一编码
	/// </summary>
	public string Code { get; set; }

	/// <summary>
	/// 密钥
	/// </summary>
	public string Secret { get; set; }

	/// <summary>
	/// 描述
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// 状态
	/// </summary>
	public AppStatus Status { get; set; } = AppStatus.Enabled;

	/// <summary>
	/// 创建时间
	/// </summary>
	public DateTime CreateTime { get; set; }

	/// <summary>
	/// 更新时间
	/// </summary>
	public DateTime UpdateTime { get; set; }

	/// <summary>
	/// 创建应用
	/// </summary>
	/// <param name="name"></param>
	/// <param name="code"></param>
	/// <param name="description"></param>
	/// <returns></returns>
	internal static AppInfo Create(string name, string code, string description)
	{
		var entity = new AppInfo
		{
			Name = name,
			Code = code.Normalize(TextCaseType.Lower),
			Secret = ObjectId.NewRandomId(DateTime.Now.Ticks),
			Description = description
		};
		entity.RaiseEvent(new AppInfoCreatedEvent());
		return entity;
	}

	/// <summary>
	/// 设置名称
	/// </summary>
	/// <param name="name"></param>
	internal void SetName(string name)
	{
		Name = name;
	}

	/// <summary>
	/// 设置描述
	/// </summary>
	/// <param name="description"></param>
	internal void SetDescription(string description)
	{
		Description = description;
	}

	/// <summary>
	/// 跟新应用信息
	/// </summary>
	/// <param name="name"></param>
	/// <param name="description"></param>
	internal void Update(string name, string description)
	{
		Name = name;
		Description = description;
		RaiseEvent(new AppInfoUpdatedEvent());
	}

	/// <summary>
	/// 设置密钥
	/// </summary>
	internal void ResetSecret()
	{
		Secret = ObjectId.NewRandomId(DateTime.Now.Ticks);
		RaiseEvent(new AppInfoSecretResetEvent());
	}

	/// <summary>
	/// 启用
	/// </summary>
	internal void Enable()
	{
		if (Status == AppStatus.Enabled)
		{
			return;
		}

		Status = AppStatus.Enabled;
		RaiseEvent(new AppInfoEnabledEvent());
	}

	/// <summary>
	/// 禁用
	/// </summary>
	internal void Disable()
	{
		if (Status == AppStatus.Disabled)
		{
			return;
		}

		Status = AppStatus.Disabled;
		RaiseEvent(new AppInfoDisableEvent());
	}
}