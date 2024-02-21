using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Mapping;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

internal interface IUserDetailUseCase : IUseCase<UserDetailInput, UserDetailOutput>;

internal record UserDetailOutput(UserDetailDto Result) : IUseCaseOutput;

internal record UserDetailInput(string Id) : IUseCaseInput;

internal class UserDetailUseCase : IUserDetailUseCase
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