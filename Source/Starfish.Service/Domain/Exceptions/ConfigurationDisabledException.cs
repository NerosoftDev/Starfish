﻿namespace Nerosoft.Starfish.Domain;

public class ConfigurationDisabledException : BadRequestException
{
	public ConfigurationDisabledException(string appId, string environment)
		: base(string.Format(Resources.IDS_ERROR_CONFIG_DISABLED, appId, environment))
	{
	}
}