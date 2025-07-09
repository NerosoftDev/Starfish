using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace Nerosoft.Starfish.Domain;

internal class UserGeneralBusiness : EditableObjectBase<UserGeneralBusiness>, IDomainService
{
	private readonly IServiceProvider _provider;

	public UserGeneralBusiness(IServiceProvider provider)
	{
		_provider = provider;
	}

	private IUserRepository _repository;
	private IUserRepository Repository => _repository ??= _provider.GetService<IUserRepository>();

	private User Aggregate { get; set; }

	public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(p => p.Id);
	public static readonly PropertyInfo<string> UsernameProperty = RegisterProperty<string>(p => p.Username);
	public static readonly PropertyInfo<string> PasswordProperty = RegisterProperty<string>(p => p.Password);
	public static readonly PropertyInfo<string> NicknameProperty = RegisterProperty<string>(p => p.Nickname);
	public static readonly PropertyInfo<string> EmailProperty = RegisterProperty<string>(p => p.Email);
	public static readonly PropertyInfo<string> PhoneProperty = RegisterProperty<string>(p => p.Phone);
	public static readonly PropertyInfo<bool> IsAdminProperty = RegisterProperty<bool>(p => p.IsAdmin);
	public static readonly PropertyInfo<bool> ReservedProperty = RegisterProperty<bool>(p => p.Reserved);

	public long Id
	{
		get => GetProperty(IdProperty);
		private set => LoadProperty(IdProperty, value);
	}

	public string Username
	{
		get => GetProperty(UsernameProperty);
		set => SetProperty(UsernameProperty, value);
	}

	public string Password
	{
		get => GetProperty(PasswordProperty);
		set => SetProperty(PasswordProperty, value);
	}

	public string Nickname
	{
		get => GetProperty(NicknameProperty);
		set => SetProperty(NicknameProperty, value);
	}

	public string Email
	{
		get => GetProperty(EmailProperty);
		set => SetProperty(EmailProperty, value);
	}

	public string Phone
	{
		get => GetProperty(PhoneProperty);
		set => SetProperty(PhoneProperty, value);
	}

	public bool IsAdmin
	{
		get => GetProperty(IsAdminProperty);
		set => SetProperty(IsAdminProperty, value);
	}

	public bool Reserved
	{
		get => GetProperty(ReservedProperty);
		set => SetProperty(ReservedProperty, value);
	}

	protected override void AddRules()
	{
		Rules.AddRule(_provider.GetServiceOrCreateInstance<UsernameAvailabilityCheckRule>());
		Rules.AddRule(new DuplicateEmailCheckRule());
		Rules.AddRule(new DuplicatePhoneCheckRule());
		Rules.AddRule(new PasswordStrengthRule());
	}

	[FactoryCreate]
	protected override Task CreateAsync(CancellationToken cancellationToken = default)
	{
		return Task.CompletedTask;
	}

	[FactoryFetch]
	protected async Task FetchAsync(long id, CancellationToken cancellationToken = default)
	{
		var user = await Repository.GetAsync(id, query => query.AsTracking(), cancellationToken);

		Aggregate = user ?? throw new UserNotFoundException(id);

		using (BypassRuleChecks)
		{
			Id = user.Id;
			Username = user.Username;
			Nickname = user.Nickname;
			Email = user.Email;
			Phone = user.Phone;
		}
	}

	[FactoryInsert]
	protected override Task InsertAsync(CancellationToken cancellationToken = default)
	{
		var user = User.Create(Username, Password);
		if (!string.IsNullOrWhiteSpace(Email))
		{
			user.SetEmail(Email);
		}

		if (!string.IsNullOrWhiteSpace(Phone))
		{
			user.SetPhone(Phone);
		}

		user.SetNickname(Nickname ?? Username);
		user.SetIsAdmin(IsAdmin);
		user.Reserved = Reserved;

		return Repository.InsertAsync(user, true, cancellationToken)
						 .ContinueWith(task =>
						 {
							 task.WaitAndUnwrapException(cancellationToken);
							 Id = task.Result.Id;
						 }, cancellationToken);
	}

	[FactoryUpdate]
	protected override Task UpdateAsync(CancellationToken cancellationToken = default)
	{
		if (!HasChangedProperties)
		{
			return Task.CompletedTask;
		}

		if (ChangedProperties.Contains(EmailProperty))
		{
			Aggregate.SetEmail(Email);
		}

		if (ChangedProperties.Contains(PhoneProperty))
		{
			Aggregate.SetPhone(Phone);
		}

		if (ChangedProperties.Contains(NicknameProperty))
		{
			Aggregate.SetNickname(Nickname);
		}

		if (ChangedProperties.Contains(IsAdminProperty))
		{
			Aggregate.SetIsAdmin(IsAdmin);
		}

		return _repository.UpdateAsync(Aggregate, true, cancellationToken);
	}

	[FactoryDelete]
	protected override Task DeleteAsync(CancellationToken cancellationToken = default)
	{
		if (Aggregate.Reserved)
		{
			throw new NotSupportedException(Resources.IDS_ERROR_USER_NOT_ALLOWED_TO_DELETE_RESERVED_USER);
		}

		return _repository.DeleteAsync(Aggregate, true, cancellationToken);
	}

	public class UsernameAvailabilityCheckRule : RuleBase
	{
		private readonly IConfiguration _configuration;

		public UsernameAvailabilityCheckRule(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public override async Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
		{
			var target = (UserGeneralBusiness)context.Target;
			if (!target.IsInsert)
			{
				return;
			}

			if (!target.Reserved)
			{
				var reserved = _configuration.GetValue<List<string>>("ReservedUsernames");
				if (reserved.Contains(target.Username, StringComparison.OrdinalIgnoreCase))
				{
					context.AddErrorResult(string.Format(Resources.IDS_ERROR_USER_USERNAME_UNAVAILABLE, target.Username));
					return;
				}
			}

			var repository = target.Repository;
			var exists = await repository.CheckUsernameExistsAsync(target.Username, cancellationToken);
			if (exists)
			{
				context.AddErrorResult(string.Format(Resources.IDS_ERROR_USER_USERNAME_UNAVAILABLE, target.Username));
			}
		}
	}

	public class DuplicateEmailCheckRule : RuleBase
	{
		public override async Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
		{
			var target = (UserGeneralBusiness)context.Target;
			if (string.IsNullOrWhiteSpace(target.Email))
			{
				return;
			}

			var changed = target.ChangedProperties.Contains(EmailProperty);
			if (!changed)
			{
				return;
			}

			var repository = target.Repository;
			var exists = await repository.CheckEmailExistsAsync(target.Email, target.Id, cancellationToken);
			if (exists)
			{
				context.AddErrorResult(string.Format(Resources.IDS_ERROR_USER_EMAIL_UNAVAILABLE, target.Email));
			}
		}
	}

	public class DuplicatePhoneCheckRule : RuleBase
	{
		public override async Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
		{
			var target = (UserGeneralBusiness)context.Target;
			if (string.IsNullOrWhiteSpace(target.Phone))
			{
				return;
			}

			var changed = target.ChangedProperties.Contains(PhoneProperty);
			if (!changed)
			{
				return;
			}

			var repository = target.Repository;
			var exists = await repository.CheckPhoneExistsAsync(target.Phone, target.Id, cancellationToken);
			if (exists)
			{
				context.AddErrorResult(string.Format(Resources.IDS_ERROR_USER_EMAIL_UNAVAILABLE, target.Email));
			}
		}
	}

	public class PasswordStrengthRule : RuleBase
	{
		public override Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
		{
			var target = (UserGeneralBusiness)context.Target;

			if (target.IsInsert)
			{
				if (string.IsNullOrWhiteSpace(target.Password))
				{
					context.AddErrorResult(Resources.IDS_ERROR_USER_RULE_PASSWORD_REQUIRED);
				}
				else if (!Regex.IsMatch(target.Password, Constants.RegexPattern.Password))
				{
					context.AddErrorResult(Resources.IDS_ERROR_USER_RULE_PASSWORD_NOT_MARCHED_RULES);
				}
			}
			else if (target.IsUpdate && target.ChangedProperties.Contains(PasswordProperty) && !Regex.IsMatch(target.Password, Constants.RegexPattern.Password))
			{
				context.AddErrorResult(Resources.IDS_ERROR_USER_RULE_PASSWORD_NOT_MARCHED_RULES);
			}

			return Task.CompletedTask;
		}
	}
}