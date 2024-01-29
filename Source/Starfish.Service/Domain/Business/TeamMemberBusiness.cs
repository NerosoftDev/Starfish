using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Business;
using Nerosoft.Starfish.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace Nerosoft.Starfish.Domain;

public class TeamMemberBusiness : EditableObjectBase<TeamMemberBusiness>
{
	[Inject]
	public ITeamRepository TeamRepository { get; set; }

	[Inject]
	public IUserRepository UserRepository { get; set; }

	private Team Aggregate { get; set; }

	public static readonly PropertyInfo<long[]> UserIdsProperty = RegisterProperty<long[]>(p => p.UserIds);

	public long[] UserIds
	{
		get => GetProperty(UserIdsProperty);
		set => SetProperty(UserIdsProperty, value);
	}

	protected override void AddRules()
	{
		Rules.AddRule(new TeamOwnerCheckRule());
		Rules.AddRule(new UserExistCheckRule());
	}

	[FactoryFetch]
	protected async Task FetchAsync(long id, CancellationToken cancellationToken = default)
	{
		var aggregate = await TeamRepository.GetAsync(id, true, [nameof(Team.Members)], cancellationToken);

		Aggregate = aggregate ?? throw new TeamNotFoundException(id);
	}

	[FactoryInsert]
	protected override Task InsertAsync(CancellationToken cancellationToken = default)
	{
		foreach (var userId in UserIds)
		{
			Aggregate.AddMember(userId);
		}

		return TeamRepository.UpdateAsync(Aggregate, true, cancellationToken);
	}

	[FactoryDelete]
	protected override Task DeleteAsync(CancellationToken cancellationToken = default)
	{
		foreach (var userId in UserIds)
		{
			Aggregate.RemoveMember(userId);
		}

		return TeamRepository.UpdateAsync(Aggregate, true, cancellationToken);
	}

	public class UserExistCheckRule : RuleBase
	{
		public override async Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
		{
			var target = (TeamMemberBusiness)context.Target;
			if (target.State == ObjectEditState.Delete)
			{
				return;
			}

			var users = await target.UserRepository.FindAsync(target.UserIds, query => query.AsNoTracking(), cancellationToken);

			var notExists = target.UserIds.Except(users.Select(t => t.Id)).ToArray();

			if (notExists.Length != 0)
			{
				context.AddErrorResult(string.Format(Resources.IDS_ERROR_USER_NOT_EXISTS, notExists.JoinAsString(", ")));
			}
		}
	}

	public class TeamOwnerCheckRule : RuleBase
	{
		public override Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
		{
			var target = (TeamMemberBusiness)context.Target;

			if (target.Aggregate.OwnerId != target.Identity.GetUserIdOfInt64())
			{
				context.AddErrorResult(Resources.IDS_ERROR_COMMON_UNAUTHORIZED_ACCESS);
			}

			return Task.CompletedTask;
		}
	}
}