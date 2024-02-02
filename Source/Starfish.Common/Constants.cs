namespace Nerosoft.Starfish.Common;

internal class Constants
{
	public static bool IsDebug
	{
		get
		{
#if DEBUG
			return true;
#else
			return false;
#endif
		}
	}
	
	public static class RequestHeaders
	{
		public const string Team = "starfish-team";
		public const string App = "starfish-app";
		public const string Secret = "starfish-secret";
		public const string Env = "starfish-env";
	}

	public static class Query
	{
		public const int Skip = 0;
		public const int Count = 20;
	}
	
	public static class Configuration
	{
		public const string FormatJson = "plain/json";
		public const string FormatText = "plain/text";
	}

	public static class RegexPattern
	{ 
		public const string Secret = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,32}$";
	}
}