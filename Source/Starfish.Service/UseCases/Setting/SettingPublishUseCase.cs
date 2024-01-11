﻿using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.UseCases;

/// <summary>
/// 配置节点发布用例接口
/// </summary>
public interface ISettingPublishUseCase : INonOutputUseCase<SettingPublishInput>;

/// <summary>
/// 配置节点发布输入
/// </summary>
/// <param name="AppId"></param>
/// <param name="Environment"></param>
/// <param name="Data"></param>
public record SettingPublishInput(long AppId, string Environment, SettingPublishDto Data) : IUseCaseInput;

/// <summary>
/// 配置节点发布用例
/// </summary>
public class SettingPublishUseCase : ISettingPublishUseCase
{
	private readonly IBus _bus;

	public SettingPublishUseCase(IBus bus)
	{
		_bus = bus;
	}

	public async Task ExecuteAsync(SettingPublishInput input, CancellationToken cancellationToken = default)
	{
		var command = new SettingPublishCommand(input.AppId, input.Environment);
		await _bus.SendAsync(command, cancellationToken);

		var @event = new SettingPublishedEvent(input.AppId, input.Environment)
		{
			Version = input.Data.Version,
			Comment = input.Data.Comment
		};
		await _bus.PublishAsync(@event, cancellationToken);
	}
}