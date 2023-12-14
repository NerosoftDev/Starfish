﻿using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 配置节点发布用例接口
/// </summary>
public interface ISettingNodePublishUseCase : IUseCase<SettingNodePublishInput>;

/// <summary>
/// 配置节点发布输入
/// </summary>
/// <param name="Id"></param>
public record SettingNodePublishInput(long Id, SettingNodePublishDto Data) : IUseCaseInput;

/// <summary>
/// 配置节点发布用例
/// </summary>
public class SettingNodePublishUseCase : ISettingNodePublishUseCase
{
	private readonly IBus _bus;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="bus"></param>
	public SettingNodePublishUseCase(IBus bus)
	{
		_bus = bus;
	}

	/// <inheritdoc />
	public async Task ExecuteAsync(SettingNodePublishInput input, CancellationToken cancellationToken = new CancellationToken())
	{
		var command = new SettingNodePublishCommand(input.Id);
		await _bus.SendAsync(command, cancellationToken);
		var @event = new SettingPublishedEvent(command.Item1)
		{
			Version = input.Data.Version,
			Description = input.Data.Description
		};
		await _bus.PublishAsync(@event, cancellationToken);
	}
}