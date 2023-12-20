using System.Text.RegularExpressions;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Common;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 应用信息
/// </summary>
public sealed class AppInfo : Aggregate<long>,
                              IHasCreateTime,
                              IHasUpdateTime
{
	private const string PATTERN_SECRET = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,32}$";

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
	/// <returns></returns>
	internal static AppInfo Create(string name, string code)
	{
		var entity = new AppInfo
		{
			Name = name,
			Code = code.Normalize(TextCaseType.Lower)
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
		if (string.Equals(Name, name, StringComparison.OrdinalIgnoreCase) && string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
		{
			return;
		}

		Name = name;
		Description = description;
		RaiseEvent(new AppInfoUpdatedEvent());
	}

	/// <summary>
	/// 设置APP密钥
	/// </summary>
	/// <param name="secret"></param>
	/// <exception cref="BadRequestException">密钥不符合规则时引发异常</exception>
	internal void SetSecret(string secret)
	{
		if (string.IsNullOrWhiteSpace(secret))
		{
			throw new BadRequestException(Resources.IDS_ERROR_APPINFO_SECRET_REQUIRED);
		}

		if (!Regex.IsMatch(secret, PATTERN_SECRET))
		{
			throw new BadRequestException(Resources.IDS_ERROR_APPINFO_SECRET_NOT_MATCHES_RULE);
		}

		Secret = Cryptography.SHA.Encrypt(secret);
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