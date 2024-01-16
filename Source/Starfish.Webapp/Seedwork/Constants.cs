namespace Nerosoft.Starfish.Webapp;

internal class Constants
{
	public static class LocalStorage
	{
		public const string AccessToken = "session_access_token";
		public const string RefreshToken = "session_refresh_token";
	}

	public static class Query
	{
		public const int DefaultSize = 20;
	}
	
	public static class Setting
	{
		public const string FormatJson = "text/json";
		public const string FormatText = "text/plain";
	}
}