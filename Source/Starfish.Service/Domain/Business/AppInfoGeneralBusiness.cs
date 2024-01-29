using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace Nerosoft.Starfish.Domain;

public class AppInfoGeneralBusiness : EditableObjectBase<AppInfoGeneralBusiness>, IDomainService
{
	[Inject]
	public IAppInfoRepository AppInfoRepository { get; set; }

	[Inject]
	public ITeamRepository TeamRepository { get; set; }

	internal AppInfo Aggregate { get; private set; }

	public static readonly PropertyInfo<string> IdProperty = RegisterProperty<string>(p => p.Id);
	public static readonly PropertyInfo<string> TeamIdProperty = RegisterProperty<string>(p => p.TeamId);
	public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
	public static readonly PropertyInfo<string> SecretProperty = RegisterProperty<string>(p => p.Secret);
	public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(p => p.Description);

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

	public string Secret
	{
		get => GetProperty(SecretProperty);
		set => SetProperty(SecretProperty, value);
	}

	public string Description
	{
		get => GetProperty(DescriptionProperty);
		set => SetProperty(DescriptionProperty, value);
	}

	[FactoryCreate]
	protected override Task CreateAsync(CancellationToken cancellationToken = default)
	{
		return Task.CompletedTask;
	}

	[FactoryFetch]
	protected async Task FetchAsync(string id, CancellationToken cancellationToken = default)
	{
		var aggregate = await AppInfoRepository.GetAsync(id, true, [], cancellationToken);

		Aggregate = aggregate ?? throw new AppInfoNotFoundException(id);

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
		await CheckPermissionAsync(TeamId, cancellationToken);

		Aggregate = AppInfo.Create(TeamId, Name);
		Aggregate.SetSecret(Secret);
		if (!string.IsNullOrWhiteSpace(Description))
		{
			Aggregate.SetDescription(Description);
		}

		await AppInfoRepository.InsertAsync(Aggregate, true, cancellationToken);
		Id = Aggregate.Id;
	}

	[FactoryUpdate]
	protected override async Task UpdateAsync(CancellationToken cancellationToken = default)
	{
		await CheckPermissionAsync(Aggregate.TeamId, cancellationToken);

		if (!HasChangedProperties)
		{
			return;
		}

		if (ChangedProperties.Contains(NameProperty))
		{
			Aggregate.SetName(Name);
		}

		if (ChangedProperties.Contains(DescriptionProperty))
		{
			Aggregate.SetDescription(Description);
		}

		await AppInfoRepository.UpdateAsync(Aggregate, true, cancellationToken);
	}

	[FactoryDelete]
	protected override async Task DeleteAsync(CancellationToken cancellationToken = default)
	{
		await CheckPermissionAsync(Aggregate.TeamId, cancellationToken);
		Aggregate.RaiseEvent(new AppInfoDeletedEvent());
		await AppInfoRepository.DeleteAsync(Aggregate, true, cancellationToken);
	}

	private async Task CheckPermissionAsync(string teamId, CancellationToken cancellationToken = default)
	{
		var team = await TeamRepository.GetAsync(teamId, false, cancellationToken);
		if (team.OwnerId != Identity.UserId)
		{
			throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
		}
	}
}