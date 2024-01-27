using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.UseCases;

public interface IGetSettingRawUseCase : IUseCase<GetSettingRawInput, GetSettingRawUseCaseOutput>;

public record GetSettingRawUseCaseOutput(string Result) : IUseCaseOutput;

public record GetSettingRawInput(long AppId, string Environment) : IUseCaseInput;

public class GetSettingRawUseCase : IGetSettingRawUseCase
{
	private readonly ISettingArchiveRepository _repository;

	public GetSettingRawUseCase(ISettingArchiveRepository repository)
	{
		_repository = repository;
	}

	public Task<GetSettingRawUseCaseOutput> ExecuteAsync(GetSettingRawInput input, CancellationToken cancellationToken = default)
	{
		return _repository.GetAsync(input.AppId, input.Environment, cancellationToken)
						   .ContinueWith(t => new GetSettingRawUseCaseOutput(t.Result.Data), cancellationToken);
	}
}
