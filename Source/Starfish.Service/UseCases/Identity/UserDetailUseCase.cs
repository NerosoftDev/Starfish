using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface IUserDetailUseCase : IUseCase<UserDetailInput, UserDetailOutput>;

public record UserDetailOutput(UserDetailDto Result) : IUseCaseOutput;

public record UserDetailInput(string Id) : IUseCaseInput;

public class UserDetailUseCase : IUserDetailUseCase
{
	private readonly IUserRepository _repository;

	public UserDetailUseCase(IUserRepository repository)
	{
		_repository = repository;
	}

	public Task<UserDetailOutput> ExecuteAsync(UserDetailInput input, CancellationToken cancellationToken = default)
	{
		return _repository.GetAsync(input.Id, query => query.AsNoTracking(), cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var dto = TypeAdapter.ProjectedAs<UserDetailDto>(task.Result);
			                  return new UserDetailOutput(dto);
		                  }, cancellationToken);
	}
}