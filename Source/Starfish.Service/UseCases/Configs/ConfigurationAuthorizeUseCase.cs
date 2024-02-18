using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;

namespace Nerosoft.Starfish.UseCases;

internal interface IConfigurationAuthorizeUseCase : IUseCase<ConfigurationAuthorizeInput, ConfigurationAuthorizeOutput>;

internal record ConfigurationAuthorizeInput(string Id, string TeamId, string Name, string Secret) : IUseCaseInput;

internal record ConfigurationAuthorizeOutput(string Id) : IUseCaseOutput;

internal class ConfigurationAuthorizeUseCase : IConfigurationAuthorizeUseCase
{
	private readonly IConfigurationRepository _repository;

	public ConfigurationAuthorizeUseCase(IConfigurationRepository repository)
	{
		_repository = repository;
	}

	public async Task<ConfigurationAuthorizeOutput> ExecuteAsync(ConfigurationAuthorizeInput input, CancellationToken cancellationToken = default)
	{
		Specification<Configuration> specification;
		if (!string.IsNullOrEmpty(input.Id))
		{
			specification = ConfigurationSpecification.IdEquals(input.Id);
		}
		else if (!string.IsNullOrEmpty(input.TeamId) && !string.IsNullOrEmpty(input.Name))
		{
			specification = new CompositeSpecification<Configuration>(PredicateOperator.AndAlso,
				ConfigurationSpecification.TeamIdEquals(input.TeamId),
				ConfigurationSpecification.NameEquals(input.Name));
		}
		else
		{
			throw new ArgumentException("Id, or TeamId and Name must be provided");
		}

		var predicate = specification.Satisfy();

		var configuration = await _repository.GetAsync(predicate, false, cancellationToken);

		if (configuration == null)
		{
			throw new ConfigurationNotFoundException();
		}

		var encryptedSecret = Cryptography.SHA.Encrypt(input.Secret);

		if (!string.Equals(configuration.Secret, encryptedSecret))
		{
			throw new UnauthorizedAccessException(Resources.IDS_ERROR_COMMON_AUTHORIZE_FAILED);
		}

		return new ConfigurationAuthorizeOutput(configuration.Id);
	}
}