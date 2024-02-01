using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 用户仓储
/// </summary>
public sealed class UserRepository : BaseRepository<DataContext, User, string>, IUserRepository
{
	/// <summary>
	/// 初始化<see cref="UserRepository"/>.
	/// </summary>
	/// <param name="provider"></param>
	public UserRepository(IContextProvider provider)
		: base(provider)
	{
	}

	/// <inheritdoc />
	public Task<User> FindByUserNameAsync(string userName, bool tracking, CancellationToken cancellationToken = default)
	{
		return GetAsync(t => t.UserName == userName, tracking, [], cancellationToken);
	}

	/// <inheritdoc />
	public Task<bool> CheckUserNameExistsAsync(string userName, CancellationToken cancellationToken = default)
	{
		var specification = UserSpecification.UserNameEquals(userName);
		var predicate = specification.Satisfy();
		return AnyAsync(predicate, null, cancellationToken);
	}

	/// <inheritdoc />
	public Task<bool> CheckEmailExistsAsync(string email, string ignoreId, CancellationToken cancellationToken = default)
	{
		ISpecification<User>[] specifications =
		[
			UserSpecification.EmailEquals(email),
			UserSpecification.IdNotEquals(ignoreId)
		];
		var predicate = new CompositeSpecification<User>(PredicateOperator.AndAlso, specifications).Satisfy();
		return AnyAsync(predicate, null, cancellationToken);
	}

	public Task<bool> CheckPhoneExistsAsync(string phone, string ignoreId, CancellationToken cancellationToken = default)
	{
		ISpecification<User>[] specifications =
		[
			UserSpecification.EmailEquals(phone),
			UserSpecification.IdNotEquals(ignoreId)
		];
		var predicate = new CompositeSpecification<User>(PredicateOperator.AndAlso, specifications).Satisfy();
		return AnyAsync(predicate, null, cancellationToken);
	}
}