﻿using Nerosoft.Euonia.Application;
using Nerosoft.Euonia.Bus;
using Nerosoft.Starfish.Application;
using Nerosoft.Starfish.Domain;
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
	private readonly ISettingNodeRepository _repository;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="bus"></param>
	/// <param name="repository"></param>
	public SettingNodePublishUseCase(IBus bus, ISettingNodeRepository repository)
	{
		_bus = bus;
		_repository = repository;
	}

	/// <inheritdoc />
	public async Task ExecuteAsync(SettingNodePublishInput input, CancellationToken cancellationToken = default)
	{
		var command = new SettingNodePublishCommand(input.Id);
		await _bus.SendAsync(command, cancellationToken);

		var @event = new SettingPublishedEvent(input.Id)
		{
			Version = input.Data.Version,
			Comment = input.Data.Comment
		};
		await _bus.PublishAsync(@event, cancellationToken);
	}
}