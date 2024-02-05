using Nerosoft.Euonia.Business;
using Nerosoft.Starfish.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace Nerosoft.Starfish.Domain;

internal class ConfigurationGeneralBusiness : EditableObjectBase<ConfigurationGeneralBusiness>
{
	[Inject]
	public IAppInfoRepository AppInfoRepository { get; set; }

	[Inject]
	public IConfigurationRepository ConfigurationRepository { get; set; }

	internal Configuration Aggregate { get; private set; }

	public static readonly PropertyInfo<string> IdProperty = RegisterProperty<string>(p => p.Id);
	public static readonly PropertyInfo<string> AppIdProperty = RegisterProperty<string>(p => p.AppId);
	public static readonly PropertyInfo<string> EnvironmentProperty = RegisterProperty<string>(p => p.Environment);
	public static readonly PropertyInfo<IDictionary<string, string>> ItemsProperty = RegisterProperty<IDictionary<string, string>>(p => p.Items);
	public static readonly PropertyInfo<string> KeyProperty = RegisterProperty<string>(p => p.Key);
	public static readonly PropertyInfo<string> ValueProperty = RegisterProperty<string>(p => p.Value);

	public string Id
	{
		get => ReadProperty(IdProperty);
		set => LoadProperty(IdProperty, value);
	}

	public string AppId
	{
		get => GetProperty(AppIdProperty);
		set => SetProperty(AppIdProperty, value);
	}

	public string Environment
	{
		get => GetProperty(EnvironmentProperty);
		set => SetProperty(EnvironmentProperty, value);
	}

	public IDictionary<string, string> Items
	{
		get => GetProperty(ItemsProperty);
		set => SetProperty(ItemsProperty, value);
	}

	public string Key
	{
		get => GetProperty(KeyProperty);
		set => SetProperty(KeyProperty, value);
	}

	public string Value
	{
		get => GetProperty(ValueProperty);
		set => SetProperty(ValueProperty, value);
	}

	protected override void AddRules()
	{
		Rules.AddRule(new DuplicateCheckRule());
	}

	[FactoryCreate]
	protected override async Task CreateAsync(CancellationToken cancellationToken = default)
	{
		await Task.CompletedTask;
	}

	[FactoryFetch]
	protected async Task FetchAsync(string appId, string environment, CancellationToken cancellationToken = default)
	{
		var aggregate = await ConfigurationRepository.GetAsync(appId, environment, true, [nameof(Configuration.Items)], cancellationToken);

		Aggregate = aggregate ?? throw new ConfigurationNotFoundException(appId, environment);

		using (BypassRuleChecks)
		{
			Id = aggregate.Id;
			AppId = aggregate.TeamId;
			Environment = aggregate.Name;
		}
	}

	[FactoryInsert]
	protected override async Task InsertAsync(CancellationToken cancellationToken = default)
	{
		var permission = await AppInfoRepository.CheckPermissionAsync(AppId, Identity.UserId, cancellationToken);

		switch (permission)
		{
			case 0:
				throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
			case 1:
			case 2:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		var appInfo = await AppInfoRepository.GetAsync(AppId, cancellationToken);

		if (appInfo == null)
		{
			throw new AppInfoNotFoundException(AppId);
		}

		if (appInfo.Status != AppStatus.Enabled)
		{
			throw new AppInfoNotEnabledException(AppId);
		}

		var aggregate = Configuration.Create(AppId, Environment, Items);
		await ConfigurationRepository.InsertAsync(aggregate, true, cancellationToken);
		Id = aggregate.Id;
	}

	[FactoryUpdate]
	protected override async Task UpdateAsync(CancellationToken cancellationToken = default)
	{
		var permission = await AppInfoRepository.CheckPermissionAsync(AppId, Identity.UserId, cancellationToken);

		switch (permission)
		{
			case 0:
				throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
			case 1:
			case 2:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		if (!HasChangedProperties)
		{
			return;
		}

		if (ChangedProperties.Contains(ItemsProperty))
		{
			Aggregate.UpdateItem(Items);
		}
		else if (ChangedProperties.Contains(KeyProperty) && ChangedProperties.Contains(ValueProperty))
		{
			Aggregate.UpdateItem(Key, Value);
		}

		await ConfigurationRepository.UpdateAsync(Aggregate, true, cancellationToken);
	}

	[FactoryDelete]
	protected override async Task DeleteAsync(CancellationToken cancellationToken = default)
	{
		var permission = await AppInfoRepository.CheckPermissionAsync(AppId, Identity.UserId, cancellationToken);

		switch (permission)
		{
			case 0:
				throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
			case 1:
			case 2:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		Aggregate.RaiseEvent(new ConfigurationDeletedEvent());
		await ConfigurationRepository.DeleteAsync(Aggregate, true, cancellationToken);
	}

	public class DuplicateCheckRule : RuleBase
	{
		public override async Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
		{
			var target = (ConfigurationGeneralBusiness)context.Target;
			if (!target.IsInsert)
			{
				return;
			}

			var exists = await target.ConfigurationRepository.ExistsAsync(target.AppId, target.Environment, cancellationToken);
			if (exists)
			{
				context.AddErrorResult(string.Format(Resources.IDS_ERROR_CONFIG_DUPLICATE, target.AppId, target.Environment));
			}
		}
	}
}