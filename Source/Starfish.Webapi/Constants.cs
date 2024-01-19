namespace Nerosoft.Starfish.Webapi;

internal class Constants
{
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
}