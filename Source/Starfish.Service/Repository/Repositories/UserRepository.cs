using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 用户仓储
/// </summary>
public sealed class UserRepository : BaseRepository<DataContext, User, long>, IUserRepository
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
	public Task<User> FindByUserNameAsync(string username, bool tracking, CancellationToken cancellationToken = default)
	{
		return GetAsync(t => t.Username == username, tracking, [], cancellationToken);
	}

	/// <inheritdoc />
	public Task<bool> CheckUsernameExistsAsync(string username, CancellationToken cancellationToken = default)
	{
		var specification = UserSpecification.UserNameEquals(username);
		var predicate = specification.Satisfy();
		return AnyAsync(predicate, null, cancellationToken);
	}

	/// <inheritdoc />
	public Task<bool> CheckEmailExistsAsync(string email, long ignoreId, CancellationToken cancellationToken = default)
	{
		ISpecification<User>[] specifications =
		[
			UserSpecification.EmailEquals(email),
			UserSpecification.IdNotEquals(ignoreId)
		];
		var predicate = new CompositeSpecification<User>(PredicateOperator.AndAlso, specifications).Satisfy();
		return AnyAsync(predicate, null, cancellationToken);
	}

	public Task<bool> CheckPhoneExistsAsync(string phone, long ignoreId, CancellationToken cancellationToken = default)
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