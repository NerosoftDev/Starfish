using System.Text.RegularExpressions;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

internal class UserPasswordBusiness : CommandObjectBase<UserPasswordBusiness>, IDomainService
{
	[Inject]
	public IUserRepository Repository { get; set; }

	[FactoryExecute]
	protected async Task ExecuteAsync(string id, string password, string actionType, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(password))
		{
			throw new ValidationException(Resources.IDS_ERROR_USER_RULE_PASSWORD_REQUIRED);
		}
		else if (!Regex.IsMatch(password, Constants.RegexPattern.Password))
		{
			throw new ValidationException(Resources.IDS_ERROR_USER_RULE_PASSWORD_NOT_MARCHED_RULES);
		}

		var aggregate = await Repository.GetAsync(id, true, cancellationToken);

		if (aggregate == null)
		{
			throw new UserNotFoundException(id);
		}

		aggregate.SetPassword(password, actionType);

		await Repository.UpdateAsync(aggregate, true, cancellationToken);
	}
}
