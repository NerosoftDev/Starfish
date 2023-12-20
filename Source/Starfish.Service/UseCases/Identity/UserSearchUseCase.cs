using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface IUserSearchUseCase : IUseCase<UserSearchInput, UserSearchOutput>;

public record UserSearchOutput(List<UserItemDto> Result) : IUseCaseOutput;

public record UserSearchInput(UserCriteria Criteria, int Page, int Size) : IUseCaseInput;

public class UserSearchUseCase : IUserSearchUseCase
{
	private readonly IUserRepository _repository;

	public UserSearchUseCase(IUserRepository repository)
	{
		_repository = repository;
	}

	public Task<UserSearchOutput> ExecuteAsync(UserSearchInput input, CancellationToken cancellationToken = default)
	{
		var specification = input.Criteria.GetSpecification();
		var predicate = specification.Satisfy();

		return _repository.FindAsync(predicate, query => query.Include(nameof(User.Roles)).OrderByDescending(t => t.Id), input.Page, input.Size, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = task.Result.ProjectedAsCollection<UserItemDto>();
			                  return new UserSearchOutput(result);
		                  }, cancellationToken);
	}
}