using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;

namespace Nerosoft.Starfish.UseCases;

public interface IGetSettingRawUseCase : IUseCase<GetSettingRawUseCaseInput, GetSettingRawUseCaseOutput>;

public record GetSettingRawUseCaseOutput(string Result) : IUseCaseOutput;

public record GetSettingRawUseCaseInput(long AppId, string Environment) : IUseCaseInput;

public class GetSettingRawUseCase : IGetSettingRawUseCase
{
	private readonly ISettingArchiveRepository _repository;

	public GetSettingRawUseCase(ISettingArchiveRepository repository)
	{
		_repository = repository;
	}

	public Task<GetSettingRawUseCaseOutput> ExecuteAsync(GetSettingRawUseCaseInput input, CancellationToken cancellationToken = default)
	{
		ISpecification<SettingArchive>[] specifications = 
		{
			SettingArchiveSpecification.AppIdEquals(input.AppId),
			SettingArchiveSpecification.EnvironmentEquals(input.Environment)
		};

		var predicate = new CompositeSpecification<SettingArchive>(PredicateOperator.AndAlso, specifications).Satisfy();

		return _repository.GetAsync(predicate, cancellationToken)
						   .ContinueWith(t => new GetSettingRawUseCaseOutput(t.Result.Data), cancellationToken);
	}
}
