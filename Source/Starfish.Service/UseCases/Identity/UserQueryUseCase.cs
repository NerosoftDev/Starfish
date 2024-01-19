﻿using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Claims;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

public interface IUserQueryUseCase : IUseCase<UserSearchInput, UserSearchOutput>;

public record UserSearchOutput(List<UserItemDto> Result) : IUseCaseOutput;

public record UserSearchInput(UserCriteria Criteria, int Skip, int Count) : IUseCaseInput;

public class UserQueryUseCase : IUserQueryUseCase
{
	private readonly IUserRepository _repository;
	private readonly UserPrincipal _identity;

	public UserQueryUseCase(IUserRepository repository, UserPrincipal identity)
	{
		_repository = repository;
		_identity = identity;
	}

	public Task<UserSearchOutput> ExecuteAsync(UserSearchInput input, CancellationToken cancellationToken = default)
	{
		if (input.Skip < 0)
		{
			throw new BadRequestException(Resources.IDS_ERROR_PARAM_SKIP_CANNOT_BE_NEGATIVE);
		}

		if (input.Count <= 0)
		{
			throw new BadRequestException(Resources.IDS_ERROR_PARAM_COUNT_MUST_GREATER_THAN_0);
		}

		if (!_identity.IsAuthenticated)
		{
			throw new AuthenticationException();
		}

		if (!_identity.IsInRole("SA"))
		{
			throw new UnauthorizedAccessException();
		}

		var specification = input.Criteria.GetSpecification();
		var predicate = specification.Satisfy();

		return _repository.FindAsync(predicate, Collator, input.Skip, input.Count, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = task.Result.ProjectedAsCollection<UserItemDto>();
			                  return new UserSearchOutput(result);
		                  }, cancellationToken);

		static IQueryable<User> Collator(IQueryable<User> query)
		{
			return query.Include(nameof(User.Roles)).OrderByDescending(t => t.Id);
		}
	}
}