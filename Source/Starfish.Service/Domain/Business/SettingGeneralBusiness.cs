using Nerosoft.Euonia.Business;
using Nerosoft.Starfish.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace Nerosoft.Starfish.Domain;

internal class SettingGeneralBusiness : EditableObjectBase<SettingGeneralBusiness>
{
	[Inject]
	public IAppInfoRepository AppInfoRepository { get; set; }

	[Inject]
	public ISettingRepository SettingRepository { get; set; }

	internal Setting Aggregate { get; private set; }

	public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(p => p.Id);
	public static readonly PropertyInfo<long> AppIdProperty = RegisterProperty<long>(p => p.AppId);
	public static readonly PropertyInfo<string> EnvironmentProperty = RegisterProperty<string>(p => p.Environment);
	public static readonly PropertyInfo<IDictionary<string, string>> ItemsProperty = RegisterProperty<IDictionary<string, string>>(p => p.Items);
	public static readonly PropertyInfo<string> KeyProperty = RegisterProperty<string>(p => p.Key);
	public static readonly PropertyInfo<string> ValueProperty = RegisterProperty<string>(p => p.Value);

	public long Id
	{
		get => ReadProperty(IdProperty);
		set => LoadProperty(IdProperty, value);
	}

	public long AppId
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
	protected async Task FetchAsync(long appId, string environment, CancellationToken cancellationToken = default)
	{
		var aggregate = await SettingRepository.GetAsync(appId, environment, true, [nameof(Setting.Items)], cancellationToken);

		Aggregate = aggregate ?? throw new SettingNotFoundException(appId, environment);

		using (BypassRuleChecks)
		{
			Id = aggregate.Id;
			AppId = aggregate.AppId;
			Environment = aggregate.Environment;
		}
	}

	[FactoryInsert]
	protected override async Task InsertAsync(CancellationToken cancellationToken = default)
	{
		var appInfo = await AppInfoRepository.GetAsync(AppId, cancellationToken);
		var aggregate = Setting.Create(AppId, Environment, Items);
		await SettingRepository.InsertAsync(aggregate, true, cancellationToken);
		Id = aggregate.Id;
	}

	[FactoryUpdate]
	protected override async Task UpdateAsync(CancellationToken cancellationToken = default)
	{
		if (!HasChangedProperties)
		{
			return;
		}

		if (ChangedProperties.Contains(ItemsProperty))
		{
			Aggregate.AddOrUpdateItem(Items);
		}
		else if (ChangedProperties.Contains(KeyProperty) && ChangedProperties.Contains(ValueProperty))
		{
			Aggregate.UpdateItem(Key, Value);
		}

		await SettingRepository.UpdateAsync(Aggregate, true, cancellationToken);
	}

	[FactoryDelete]
	protected override Task DeleteAsync(CancellationToken cancellationToken = default)
	{
		Aggregate.RaiseEvent(new SettingDeletedEvent());
		return SettingRepository.DeleteAsync(Aggregate, true, cancellationToken);
	}

	public class DuplicateCheckRule : RuleBase
	{
		public override async Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
		{
			var target = (SettingGeneralBusiness)context.Target;
			if (!target.IsInsert)
			{
				return;
			}

			var exists = await target.SettingRepository.ExistsAsync(target.AppId, target.Environment, cancellationToken);
			if (exists)
			{
				context.AddErrorResult(string.Format(Resources.IDS_ERROR_SETTING_DUPLICATE, target.AppId, target.Environment));
			}
		}
	}
}