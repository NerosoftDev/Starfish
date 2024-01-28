using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.UseCases;

public interface IGetConfigurationRawUseCase : IUseCase<GetConfigurationRawInput, GetConfigurationRawUseCaseOutput>;

public record GetConfigurationRawUseCaseOutput(string Result) : IUseCaseOutput;

public record GetConfigurationRawInput(long AppId, string Environment) : IUseCaseInput;

public class GetConfigurationRawUseCase : IGetConfigurationRawUseCase
{
	private readonly IConfigurationArchiveRepository _repository;

	public GetConfigurationRawUseCase(IConfigurationArchiveRepository repository)
	{
		_repository = repository;
	}

	public Task<GetConfigurationRawUseCaseOutput> ExecuteAsync(GetConfigurationRawInput input, CancellationToken cancellationToken = default)
	{
		return _repository.GetAsync(input.AppId, input.Environment, cancellationToken)
						   .ContinueWith(t => new GetConfigurationRawUseCaseOutput(t.Result.Data), cancellationToken);
	}
}
