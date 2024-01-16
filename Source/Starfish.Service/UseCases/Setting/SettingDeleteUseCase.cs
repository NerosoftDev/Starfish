﻿using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 删除配置节点用例接口
/// </summary>
public interface ISettingDeleteUseCase : INonOutputUseCase<SettingDeleteInput>;

/// <summary>
/// 删除配置节点输入
/// </summary>
/// <param name="AppId"></param>
/// <param name="Environment"></param>
public record SettingDeleteInput(long AppId, string Environment) : IUseCaseInput;

/// <summary>
/// 删除配置节点用例
/// </summary>
public class SettingDeleteUseCase : ISettingDeleteUseCase
{
	private readonly IBus _bus;

	public SettingDeleteUseCase(IBus bus)
	{
		_bus = bus;
	}

	public Task ExecuteAsync(SettingDeleteInput input, CancellationToken cancellationToken = default)
	{
		var command = new SettingDeleteCommand(input.AppId, input.Environment);
		return _bus.SendAsync(command, cancellationToken);
	}
}