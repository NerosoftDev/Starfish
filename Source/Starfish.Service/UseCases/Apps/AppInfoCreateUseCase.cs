using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 创建应用信息用例接口
/// </summary>
public interface IAppInfoCreateUseCase : IUseCase<AppInfoCreateInput, AppInfoCreateOutput>;

/// <summary>
/// 创建应用信息用例输出
/// </summary>
/// <param name="Id"></param>
public record AppInfoCreateOutput(string Id) : IUseCaseOutput;

/// <summary>
/// 创建应用信息用例输入
/// </summary>
/// <param name="Data"></param>
public record AppInfoCreateInput(AppInfoCreateDto Data) : IUseCaseInput;

/// <summary>
/// 创建应用信息用例
/// </summary>
public class AppInfoCreateUseCase : IAppInfoCreateUseCase
{
	private readonly IBus _bus;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="bus"></param>
	public AppInfoCreateUseCase(IBus bus)
	{
		_bus = bus;
	}

	/// <inheritdoc />
	public Task<AppInfoCreateOutput> ExecuteAsync(AppInfoCreateInput input, CancellationToken cancellationToken = default)
	{
		var command = new AppInfoCreateCommand(input.Data);
		return _bus.SendAsync<AppInfoCreateCommand, string>(command, cancellationToken)
		           .ContinueWith(task =>
		           {
			           task.WaitAndUnwrapException(cancellationToken);
			           return new AppInfoCreateOutput(task.Result);
		           }, cancellationToken);
	}
}