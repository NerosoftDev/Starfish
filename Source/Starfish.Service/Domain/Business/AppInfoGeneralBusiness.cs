using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Repository;
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

	public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(p => p.Id);
	public static readonly PropertyInfo<long> TeamIdProperty = RegisterProperty<long>(p => p.TeamId);
	public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
	public static readonly PropertyInfo<string> CodeProperty = RegisterProperty<string>(p => p.Code);
	public static readonly PropertyInfo<string> SecretProperty = RegisterProperty<string>(p => p.Secret);
	public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(p => p.Description);

	public long Id
	{
		get => ReadProperty(IdProperty);
		set => LoadProperty(IdProperty, value);
	}

	public long TeamId
	{
		get => GetProperty(TeamIdProperty);
		set => SetProperty(TeamIdProperty, value);
	}

	public string Name
	{
		get => GetProperty(NameProperty);
		set => SetProperty(NameProperty, value);
	}

	public string Code
	{
		get => GetProperty(CodeProperty);
		set => SetProperty(CodeProperty, value);
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

	protected override void AddRules()
	{
		Rules.AddRule(new CodeDuplicateCheckRule());
	}

	[FactoryCreate]
	protected override Task CreateAsync(CancellationToken cancellationToken = default)
	{
		return Task.CompletedTask;
	}

	[FactoryFetch]
	protected async Task FetchAsync(long id, CancellationToken cancellationToken = default)
	{
		var aggregate = await AppInfoRepository.GetAsync(id, true, [], cancellationToken);

		Aggregate = aggregate ?? throw new AppInfoNotFoundException(id);

		using (BypassRuleChecks)
		{
			Id = aggregate.Id;
			TeamId = aggregate.TeamId;
			Name = aggregate.Name;
			Code = aggregate.Code;
			Description = aggregate.Description;
		}
	}

	[FactoryInsert]
	protected override async Task InsertAsync(CancellationToken cancellationToken = default)
	{
		await CheckPermissionAsync(TeamId, cancellationToken);

		Aggregate = AppInfo.Create(TeamId, Name, Code);
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

		if (ChangedProperties.Contains(CodeProperty))
		{
			Aggregate.SetCode(Code);
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

	private async Task CheckPermissionAsync(long teamId, CancellationToken cancellationToken = default)
	{
		var team = await TeamRepository.GetAsync(teamId, false, cancellationToken);
		if (team.OwnerId != Identity.GetUserIdOfInt64())
		{
			throw new UnauthorizedAccessException(Resources.IDS_ERROR_TEAM_ONLY_ALLOW_OWNER_UPDATE);
		}
	}

	private class CodeDuplicateCheckRule : RuleBase
	{
		public override async Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
		{
			var target = (AppInfoGeneralBusiness)context.Target;

			if (!target.ChangedProperties.Contains(CodeProperty))
			{
				return;
			}

			var predicate = AppInfoSpecification.CodeEquals(target.Code).Satisfy();

			var exists = await target.AppInfoRepository.AnyAsync(predicate, cancellationToken);
			if (exists)
			{
				context.AddErrorResult(Resources.IDS_ERROR_APPINFO_CODE_UNAVAILABLE);
			}
		}
	}
}