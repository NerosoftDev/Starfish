using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace Nerosoft.Starfish.Domain;

public class TeamGeneralBusiness : EditableObjectBase<TeamGeneralBusiness>, IDomainService
{
	[Inject]
	public ITeamRepository TeamRepository { get; set; }

	private Team Aggregate { get; set; }

	public static readonly PropertyInfo<int> IdProperty = RegisterProperty<int>(p => p.Id);
	public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
	public static readonly PropertyInfo<string> AliasProperty = RegisterProperty<string>(p => p.Alias);
	public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(p => p.Description);

	public int Id
	{
		get => GetProperty(IdProperty);
		private set => LoadProperty(IdProperty, value);
	}

	public string Name
	{
		get => GetProperty(NameProperty);
		set => SetProperty(NameProperty, value);
	}

	public string Alias
	{
		get => GetProperty(AliasProperty);
		set => SetProperty(AliasProperty, value);
	}

	public string Description
	{
		get => GetProperty(DescriptionProperty);
		set => SetProperty(DescriptionProperty, value);
	}

	protected override void AddRules()
	{
		Rules.AddRule(new TeamOwnerCheckRule());
	}

	[FactoryCreate]
	protected override Task CreateAsync(CancellationToken cancellationToken = default)
	{
		return Task.CompletedTask;
	}

	[FactoryFetch]
	protected async Task FetchAsync(int id, CancellationToken cancellationToken = default)
	{
		var aggregate = await TeamRepository.GetAsync(id, true, [], cancellationToken);

		Aggregate = aggregate ?? throw new TeamNotFoundException(id);

		using (BypassRuleChecks)
		{
			Id = aggregate.Id;
			Name = aggregate.Name;
			Alias = aggregate.Alias;
			Description = aggregate.Description;
		}
	}

	[FactoryInsert]
	protected override Task InsertAsync(CancellationToken cancellationToken = default)
	{
		var team = Team.Create(Name, Description, Identity.GetUserIdOfInt32());
		if (!string.IsNullOrWhiteSpace(Alias))
		{
			team.SetAlias(Alias);
		}

		return TeamRepository.InsertAsync(team, true, cancellationToken)
		                     .ContinueWith(task =>
		                     {
			                     task.WaitAndUnwrapException(cancellationToken);
			                     Id = task.Result.Id;
		                     }, cancellationToken);
	}

	[FactoryUpdate]
	protected override Task UpdateAsync(CancellationToken cancellationToken = default)
	{
		if (ChangedProperties.Contains(NameProperty))
		{
			Aggregate.SetName(Name);
		}

		if (ChangedProperties.Contains(DescriptionProperty))
		{
			Aggregate.SetDescription(Description);
		}

		if (ChangedProperties.Contains(AliasProperty))
		{
			Aggregate.SetAlias(Alias);
		}

		return TeamRepository.UpdateAsync(Aggregate, true, cancellationToken);
	}

	public class TeamOwnerCheckRule : RuleBase
	{
		public override Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
		{
			var target = (TeamGeneralBusiness)context.Target;

			if (!target.IsInsert)
			{
				if (target.Aggregate.OwnerId != target.Identity.GetUserIdOfInt32())
				{
					context.AddErrorResult(Resources.IDS_ERROR_TEAM_ONLY_ALLOW_OWNER_UPDATE);
				}
			}

			{
			}

			return Task.CompletedTask;
		}
	}
}