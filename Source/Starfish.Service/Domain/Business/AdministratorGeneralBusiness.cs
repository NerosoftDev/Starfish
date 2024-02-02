using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Business;
using Nerosoft.Starfish.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace Nerosoft.Starfish.Domain;

internal sealed class AdministratorGeneralBusiness : EditableObjectBase<AdministratorGeneralBusiness>
{
	[Inject]
	public IUserRepository UserRepository { get; set; }

	[Inject]
	public IAdministratorRepository AdministratorRepository { get; set; }

	public Administrator Aggregate { get; private set; }

	public static readonly PropertyInfo<string> UserIdProperty = RegisterProperty<string>(p => p.UserId);

	public string UserId
	{
		get => GetProperty(UserIdProperty);
		set => SetProperty(UserIdProperty, value);
	}

	public static readonly PropertyInfo<HashSet<string>> RolesProperty = RegisterProperty<HashSet<string>>(p => p.Roles, []);

	public HashSet<string> Roles
	{
		get => GetProperty(RolesProperty);
		set => SetProperty(RolesProperty, value);
	}

	protected override void AddRules()
	{
		Rules.AddRule(new UserCheckRule());
	}

	public void SetRoles(IEnumerable<string> roles)
	{
		if (Roles.SetEquals(roles))
		{
			return;
		}

		Roles.Clear();
		if (roles.Any())
		{
			foreach (var role in roles)
			{
				Roles.Add(role);
			}
		}

		ChangedProperties.Add(RolesProperty);
	}

	[FactoryFetch]
	internal async Task FetchAsync(string userId, CancellationToken cancellationToken = default)
	{
		var aggregate = await AdministratorRepository.GetAsync(t => t.UserId == userId, query => query.AsTracking(), cancellationToken);

		if (aggregate != null)
		{
			using (BypassRuleChecks)
			{
				UserId = aggregate.UserId;
				Roles = [..aggregate.Roles.Split(",")];
			}

			Aggregate = aggregate;
		}
	}

	[FactoryInsert]
	protected override async Task InsertAsync(CancellationToken cancellationToken = default)
	{
		Aggregate = new Administrator
		{
			UserId = UserId,
			Roles = Roles.JoinAsString(",")
		};

		await AdministratorRepository.InsertAsync(Aggregate, true, cancellationToken);
	}

	[FactoryUpdate]
	protected override async Task UpdateAsync(CancellationToken cancellationToken = default)
	{
		if (ChangedProperties.Contains(RolesProperty))
		{
			Aggregate.Roles = Roles.JoinAsString(",");
		}

		await AdministratorRepository.UpdateAsync(Aggregate, true, cancellationToken);
	}

	[FactoryDelete]
	protected override async Task DeleteAsync(CancellationToken cancellationToken = default)
	{
		if (Aggregate == null)
		{
			throw new NotFoundException();
		}

		await AdministratorRepository.DeleteAsync(Aggregate, true, cancellationToken);
	}

	public class UserCheckRule : RuleBase
	{
		public override async Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
		{
			var target = (AdministratorGeneralBusiness)context.Target;
			if (target.ChangedProperties.Contains(UserIdProperty))
			{
				var exists = await target.UserRepository.AnyAsync(t => t.Id == target.UserId, query => query.AsNoTracking(), cancellationToken);
				if (exists)
				{
					context.AddErrorResult(string.Format(Resources.IDS_ERROR_USER_NOT_EXISTS, target.UserId));
				}
			}
		}
	}
}