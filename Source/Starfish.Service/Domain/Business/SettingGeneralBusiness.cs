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
	public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(p => p.Description);
	public static readonly PropertyInfo<SettingStatus> StatusProperty = RegisterProperty<SettingStatus>(p => p.Status);
	public static readonly PropertyInfo<IDictionary<string, string>> NodesProperty = RegisterProperty<IDictionary<string, string>>(p => p.Nodes);

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

	public string Description
	{
		get => GetProperty(DescriptionProperty);
		set => SetProperty(DescriptionProperty, value);
	}

	public SettingStatus Status
	{
		get => GetProperty(StatusProperty);
		set => SetProperty(StatusProperty, value);
	}

	public IDictionary<string, string> Nodes
	{
		get => GetProperty(NodesProperty);
		set => SetProperty(NodesProperty, value);
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
	protected async Task FetchAsync(long id, CancellationToken cancellationToken = default)
	{
		var aggregate = await SettingRepository.GetAsync(id, false, Array.Empty<string>(), cancellationToken);

		Aggregate = aggregate ?? throw new SettingNotFoundException(id);

		using (BypassRuleChecks)
		{
			Id = aggregate.Id;
			AppId = aggregate.AppId;
			Environment = aggregate.Environment;
			Description = aggregate.Description;
		}
	}

	[FactoryInsert]
	protected override async Task InsertAsync(CancellationToken cancellationToken = default)
	{
		var appInfo = await AppInfoRepository.GetAsync(AppId, cancellationToken);
		var aggregate = Setting.Create(AppId, appInfo.Code, Environment, Description, Nodes);
		await SettingRepository.InsertAsync(aggregate, true, cancellationToken);
		Id = aggregate.Id;
	}

	[FactoryUpdate]
	protected override async Task UpdateAsync(CancellationToken cancellationToken = default)
	{
		if (ChangedProperties.Contains(DescriptionProperty))
		{
			Aggregate.SetDescription(Description);
		}

		if (ChangedProperties.Contains(StatusProperty))
		{
			Aggregate.SetStatus(Status);
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
		public override async Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = new CancellationToken())
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