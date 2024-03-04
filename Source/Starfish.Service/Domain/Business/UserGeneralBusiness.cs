using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
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

	public static readonly PropertyInfo<string> IdProperty = RegisterProperty<string>(p => p.Id);
	public static readonly PropertyInfo<string> UserNameProperty = RegisterProperty<string>(p => p.UserName);
	public static readonly PropertyInfo<string> PasswordProperty = RegisterProperty<string>(p => p.Password);
	public static readonly PropertyInfo<string> NickNameProperty = RegisterProperty<string>(p => p.NickName);
	public static readonly PropertyInfo<string> EmailProperty = RegisterProperty<string>(p => p.Email);
	public static readonly PropertyInfo<string> PhoneProperty = RegisterProperty<string>(p => p.Phone);
	public static readonly PropertyInfo<bool> IsAdminProperty = RegisterProperty<bool>(p => p.IsAdmin);
	public static readonly PropertyInfo<bool> ReservedProperty = RegisterProperty<bool>(p => p.Reserved);

	public string Id
	{
		get => GetProperty(IdProperty);
		private set => LoadProperty(IdProperty, value);
	}

	public string UserName
	{
		get => GetProperty(UserNameProperty);
		set => SetProperty(UserNameProperty, value);
	}

	public string Password
	{
		get => GetProperty(PasswordProperty);
		set => SetProperty(PasswordProperty, value);
	}

	public string NickName
	{
		get => GetProperty(NickNameProperty);
		set => SetProperty(NickNameProperty, value);
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
		Rules.AddRule(new DuplicateUserNameCheckRule());
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
	protected async Task FetchAsync(string id, CancellationToken cancellationToken = default)
	{
		var user = await Repository.GetAsync(id, query => query.AsTracking(), cancellationToken);

		Aggregate = user ?? throw new UserNotFoundException(id);

		using (BypassRuleChecks)
		{
			Id = user.Id;
			UserName = user.UserName;
			NickName = user.NickName;
			Email = user.Email;
			Phone = user.Phone;
		}
	}

	[FactoryInsert]
	protected override Task InsertAsync(CancellationToken cancellationToken = default)
	{
		var user = User.Create(UserName, Password);
		if (!string.IsNullOrWhiteSpace(Email))
		{
			user.SetEmail(Email);
		}

		if (!string.IsNullOrWhiteSpace(Phone))
		{
			user.SetPhone(Phone);
		}

		user.SetNickName(NickName ?? UserName);
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
		if (ChangedProperties.Contains(EmailProperty))
		{
			Aggregate.SetEmail(Email);
		}

		if (ChangedProperties.Contains(PhoneProperty))
		{
			Aggregate.SetPhone(Phone);
		}

		if (ChangedProperties.Contains(NickNameProperty))
		{
			Aggregate.SetNickName(NickName);
		}

		if (ChangedProperties.Contains(PasswordProperty))
		{
			Aggregate.ChangePassword(Password);
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

	public class DuplicateUserNameCheckRule : RuleBase
	{
		public override async Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
		{
			var target = (UserGeneralBusiness)context.Target;
			if (!target.IsInsert)
			{
				return;
			}

			var repository = target.Repository;
			var exists = await repository.CheckUserNameExistsAsync(target.UserName, cancellationToken);
			if (exists)
			{
				context.AddErrorResult(string.Format(Resources.IDS_ERROR_USER_USERNAME_UNAVAILABLE, target.UserName));
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
		private const string REGEX_PATTERN = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[\x00-\xff]{8,32}$";

		public override Task ExecuteAsync(IRuleContext context, CancellationToken cancellationToken = default)
		{
			var target = (UserGeneralBusiness)context.Target;

			if (target.IsInsert)
			{
				if (string.IsNullOrWhiteSpace(target.Password))
				{
					context.AddErrorResult(Resources.IDS_ERROR_USER_RULE_PASSWORD_REQUIRED);
				}
				else if (!Regex.IsMatch(target.Password, REGEX_PATTERN))
				{
					context.AddErrorResult(Resources.IDS_ERROR_USER_RULE_PASSWORD_NOT_MARCHED_RULES);
				}
			}
			else if (target.IsUpdate && target.ChangedProperties.Contains(PasswordProperty) && !Regex.IsMatch(target.Password, REGEX_PATTERN))
			{
				context.AddErrorResult(Resources.IDS_ERROR_USER_RULE_PASSWORD_NOT_MARCHED_RULES);
			}

			return Task.CompletedTask;
		}
	}
}