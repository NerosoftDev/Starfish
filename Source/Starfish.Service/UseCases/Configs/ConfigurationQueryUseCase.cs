using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

internal interface IConfigurationQueryUseCase : IUseCase<ConfigurationQueryInput, ConfigurationQueryOutput>;

internal record ConfigurationQueryInput(ConfigurationCriteria Criteria, int Skip, int Count) : IUseCaseInput;

internal record ConfigurationQueryOutput(List<ConfigurationDto> Result) : IUseCaseOutput;

internal class ConfigurationQueryUseCase : IConfigurationQueryUseCase
{
	private readonly IConfigurationRepository _repository;

	public ConfigurationQueryUseCase(IConfigurationRepository repository)
	{
		_repository = repository;
	}

	public Task<ConfigurationQueryOutput> ExecuteAsync(ConfigurationQueryInput input, CancellationToken cancellationToken = default)
	{
		var specification = input.Criteria.GetSpecification();
		var predicate = specification.Satisfy();
		return _repository.FindAsync(predicate, input.Skip, input.Count, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  var result = task.Result.ProjectedAsCollection<ConfigurationDto>();
			                  return new ConfigurationQueryOutput(result);
		                  }, cancellationToken);
	}
}