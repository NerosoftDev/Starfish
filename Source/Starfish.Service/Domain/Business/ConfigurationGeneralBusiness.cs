using Nerosoft.Euonia.Business;
using Nerosoft.Starfish.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace Nerosoft.Starfish.Domain;

internal class ConfigurationGeneralBusiness : EditableObjectBase<ConfigurationGeneralBusiness>
{
	[Inject]
	public ITeamRepository TeamRepository { get; set; }

	[Inject]
	public IConfigurationRepository ConfigurationRepository { get; set; }

	internal Configuration Aggregate { get; private set; }

	public static readonly PropertyInfo<string> IdProperty = RegisterProperty<string>(p => p.Id);
	public static readonly PropertyInfo<string> TeamIdProperty = RegisterProperty<string>(p => p.TeamId);
	public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
	public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(p => p.Description);
	public static readonly PropertyInfo<string> SecretProperty = RegisterProperty<string>(p => p.Secret);

	public string Id
	{
		get => ReadProperty(IdProperty);
		set => LoadProperty(IdProperty, value);
	}

	public string TeamId
	{
		get => GetProperty(TeamIdProperty);
		set => SetProperty(TeamIdProperty, value);
	}

	public string Name
	{
		get => GetProperty(NameProperty);
		set => SetProperty(NameProperty, value);
	}

	public string Description
	{
		get => GetProperty(DescriptionProperty);
		set => SetProperty(DescriptionProperty, value);
	}

	public string Secret
	{
		get => GetProperty(SecretProperty);
		set => SetProperty(SecretProperty, value);
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
	protected async Task FetchAsync(string id, CancellationToken cancellationToken = default)
	{
		var aggregate = await ConfigurationRepository.GetAsync(id, true, [nameof(Configuration.Items)], cancellationToken);

		Aggregate = aggregate ?? throw new ConfigurationNotFoundException(id);

		using (BypassRuleChecks)
		{
			Id = aggregate.Id;
			TeamId = aggregate.TeamId;
			Name = aggregate.Name;
			Description = aggregate.Description;
		}
	}

	[FactoryInsert]
	protected override async Task InsertAsync(CancellationToken cancellationToken = default)
	{
		var permission = await TeamRepository.CheckPermissionAsync(TeamId, Identity.UserId, cancellationToken);

		switch (permission)
		{
			case PermissionState.None: // 无权限
				throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
			case PermissionState.Edit:
				break;
			case PermissionState.Read:
				throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
			default:
				throw new ArgumentOutOfRangeException();
		}

		var aggregate = Configuration.Create(TeamId, Name);
		if (!string.IsNullOrWhiteSpace(Secret))
		{
			aggregate.SetSecret(Secret);
		}

		await ConfigurationRepository.InsertAsync(aggregate, true, cancellationToken);
		Id = aggregate.Id;
	}

	[FactoryUpdate]
	protected override async Task UpdateAsync(CancellationToken cancellationToken = default)
	{
		var permission = await TeamRepository.CheckPermissionAsync(Aggregate.TeamId, Identity.UserId, cancellationToken);

		switch (permission)
		{
			case PermissionState.None: // 无权限
				throw new ConfigurationNotFoundException(Id);
			case PermissionState.Edit:
				break;
			case PermissionState.Read:
				throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
			default:
				throw new ArgumentOutOfRangeException();
		}

		if (!HasChangedProperties)
		{
			return;
		}

		if (ChangedProperties.Contains(NameProperty))
		{
			Aggregate.SetName(Name);
		}

		if (ChangedProperties.Contains(SecretProperty))
		{
			Aggregate.SetSecret(Secret);
		}

		if (ChangedProperties.Contains(DescriptionProperty))
		{
			Aggregate.SetDescription(Description);
		}

		Aggregate.RaiseEvent(new ConfigurationUpdatedEvent());

		await ConfigurationRepository.UpdateAsync(Aggregate, true, cancellationToken);
	}

	[FactoryDelete]
	protected override async Task DeleteAsync(CancellationToken cancellationToken = default)
	{
		var permission = await TeamRepository.CheckPermissionAsync(Aggregate.TeamId, Identity.UserId, cancellationToken);

		switch (permission)
		{
			case PermissionState.None: // 无权限
				throw new ConfigurationNotFoundException(Id);
			case PermissionState.Edit:
				break;
			case PermissionState.Read:
				throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
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

			var exists = await target.ConfigurationRepository.ExistsAsync(target.TeamId, target.Name, cancellationToken);
			if (exists)
			{
				context.AddErrorResult(string.Format(Resources.IDS_ERROR_CONFIG_DUPLICATE, target.TeamId, target.Name));
			}
		}
	}
}