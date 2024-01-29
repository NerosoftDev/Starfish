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

	public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(p => p.Id);
	public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
	public static readonly PropertyInfo<string> AliasProperty = RegisterProperty<string>(p => p.Alias);
	public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(p => p.Description);

	public long Id
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
	protected async Task FetchAsync(long id, CancellationToken cancellationToken = default)
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
		var team = Team.Create(Name, Description, Identity.GetUserIdOfInt64());
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
				if (target.Aggregate.OwnerId != target.Identity.GetUserIdOfInt64())
				{
					context.AddErrorResult(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
				}
			}

			{
			}

			return Task.CompletedTask;
		}
	}
}