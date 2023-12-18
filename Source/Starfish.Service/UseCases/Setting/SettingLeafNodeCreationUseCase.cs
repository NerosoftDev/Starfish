using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 应用配置节点更新用例接口
/// </summary>
public interface ISettingLeafNodeCreateUseCase : IUseCase<SettingLeafNodeCreationInput, SettingLeafNodeCreationOutput>;

/// <summary>
/// 应用配置子节点创建输出
/// </summary>
/// <param name="Result"></param>
public record SettingLeafNodeCreationOutput(long Result) : IUseCaseOutput;

/// <summary>
/// 应用配置子节点创建输入
/// </summary>
/// <param name="ParentId"></param>
/// <param name="Type"></param>
/// <param name="Data"></param>
public record SettingLeafNodeCreationInput(long ParentId, SettingNodeType Type, SettingNodeCreateDto Data) : IUseCaseInput;

/// <summary>
/// 应用配置子节点创建用例
/// </summary>
public class SettingLeafNodeCreateUseCase : ISettingLeafNodeCreateUseCase
{
	private readonly IBus _bus;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="bus"></param>
	public SettingLeafNodeCreateUseCase(IBus bus)
	{
		_bus = bus;
	}

	/// <inheritdoc />
	public Task<SettingLeafNodeCreationOutput> ExecuteAsync(SettingLeafNodeCreationInput input, CancellationToken cancellationToken = default)
	{
		var command = new SettingLeafNodeCreateCommand(input.ParentId, input.Type, input.Data);
		return _bus.SendAsync<SettingLeafNodeCreateCommand, long>(command, cancellationToken)
		           .ContinueWith(t => new SettingLeafNodeCreationOutput(t.Result), cancellationToken);
	}
}