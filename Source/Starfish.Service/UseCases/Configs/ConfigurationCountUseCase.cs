using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

internal interface IConfigurationCountUseCase : IUseCase<ConfigurationCountInput, ConfigurationCountOutput>;

internal record ConfigurationCountInput(ConfigurationCriteria Criteria) : IUseCaseInput;

internal record ConfigurationCountOutput(int Result) : IUseCaseOutput;

internal class ConfigurationCountUseCase : IConfigurationCountUseCase
{
	private readonly IConfigurationRepository _repository;

	public ConfigurationCountUseCase(IConfigurationRepository repository)
	{
		_repository = repository;
	}

	public Task<ConfigurationCountOutput> ExecuteAsync(ConfigurationCountInput input, CancellationToken cancellationToken = default)
	{
		var specification = input.Criteria.GetSpecification();
		var predicate = specification.Satisfy();
		return _repository.CountAsync(predicate, cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  return new ConfigurationCountOutput(task.Result);
		                  }, cancellationToken);
	}
}