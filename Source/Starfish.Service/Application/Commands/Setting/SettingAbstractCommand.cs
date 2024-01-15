﻿using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

public abstract class SettingAbstractCommand : Command
{
	protected SettingAbstractCommand(long appId, string environment)
	{
		AppId = appId;
		Environment = environment;
	}

	/// <summary>
	/// 应用Id
	/// </summary>
	public long AppId { get; set; }

	/// <summary>
	/// 环境名称
	/// </summary>
	public string Environment { get; set; }
}