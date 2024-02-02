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

	public static readonly PropertyInfo<string> IdProperty = RegisterProperty<string>(p => p.Id);
	public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
	public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(p => p.Description);

	public string Id
	{
		get => GetProperty(IdProperty);
		private set => LoadProperty(IdProperty, value);
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
	protected async Task FetchAsync(string id, CancellationToken cancellationToken = default)
	{
		var aggregate = await TeamRepository.GetAsync(id, true, [], cancellationToken);

		Aggregate = aggregate ?? throw new TeamNotFoundException(id);

		using (BypassRuleChecks)
		{
			Id = aggregate.Id;
			Name = aggregate.Name;
			Description = aggregate.Description;
		}
	}

	[FactoryInsert]
	protected override Task InsertAsync(CancellationToken cancellationToken = default)
	{
		var team = Team.Create(Name, Description, Identity.UserId);
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

		return TeamRepository.UpdateAsync(Aggregate, true, cancellationToken);
	}

	public class TeamOwnerCheckRule : RuleBase
	{
		public override Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
		{
			var target = (TeamGeneralBusiness)context.Target;

			if (!target.IsInsert)
			{
				if (target.Aggregate.OwnerId != target.Identity.UserId)
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