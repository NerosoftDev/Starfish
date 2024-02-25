using Nerosoft.Euonia.Application;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.UseCases;

internal interface IGetConfigurationArchiveUseCase : IUseCase<GetConfigurationArchiveInput, GetConfigurationArchiveOutput>;

internal record GetConfigurationArchiveOutput(string Result) : IUseCaseOutput;

internal record GetConfigurationArchiveInput(string Id) : IUseCaseInput;

internal class GetConfigurationArchiveUseCase : IGetConfigurationArchiveUseCase
{
	private readonly IConfigurationRepository _repository;

	public GetConfigurationArchiveUseCase(IConfigurationRepository repository)
	{
		_repository = repository;
	}

	public Task<GetConfigurationArchiveOutput> ExecuteAsync(GetConfigurationArchiveInput input, CancellationToken cancellationToken = default)
	{
		return _repository.GetAsync(input.Id, false, [nameof(Configuration.Archive)], cancellationToken)
		                  .ContinueWith(task =>
		                  {
			                  task.WaitAndUnwrapException(cancellationToken);
			                  if (task.Result == null)
			                  {
				                  throw new NotFoundException("配置不存在");
			                  }

			                  return new GetConfigurationArchiveOutput(task.Result.Archive?.Data);
		                  }, cancellationToken);
	}
}