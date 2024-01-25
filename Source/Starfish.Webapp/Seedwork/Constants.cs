namespace Nerosoft.Starfish.Webapp;

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



	public static class LocalStorage
	{
		public const string AccessToken = "session_access_token";
		public const string RefreshToken = "session_refresh_token";
	}

	public static class Query
	{
		public const int Skip = 0;
		public const int Count = 20;
	}

	public static class Setting
	{
		public const string FormatJson = "plain/json";
		public const string FormatText = "plain/text";
	}

	public static class Message
	{
		public const string ExceptionThrown = "exception_thrown";
	}
}