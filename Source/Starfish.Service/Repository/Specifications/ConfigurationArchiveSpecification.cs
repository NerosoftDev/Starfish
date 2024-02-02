﻿using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

internal static class ConfigurationArchiveSpecification
{
	public static Specification<ConfigurationArchive> AppIdEquals(string appId)
	{
		return new DirectSpecification<ConfigurationArchive>(x => x.AppId == appId);
	}

	public static Specification<ConfigurationArchive> EnvironmentEquals(string environment)
	{
		environment = environment.Normalize(TextCaseType.Upper);
		return new DirectSpecification<ConfigurationArchive>(x => x.Environment == environment);
	}
}