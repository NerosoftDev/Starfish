using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface IUserCountUseCase : IUseCase<UserCriteria, int>;

public class UserCountUseCase : IUserCountUseCase
{
	private readonly IUserRepository _repository;

	public UserCountUseCase(IUserRepository repository)
	{
		_repository = repository;
	}

	public Task<int> ExecuteAsync(UserCriteria input, CancellationToken cancellationToken = default)
	{
		var specification = input.GetSpecification();
		var predicate = specification.Satisfy();
		return _repository.CountAsync(predicate, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  return task.Result;
		                  }, cancellationToken);
	}
}