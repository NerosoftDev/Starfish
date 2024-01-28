using CommunityToolkit.Mvvm.Messaging;
using Nerosoft.Starfish.Webapp.Rest;
using Refit;

namespace Nerosoft.Starfish.Webapp;

internal static class ExceptionExtensions
{
	public static Exception UnwrapException(this Exception exception)
	{
		if (exception is AggregateException aggregateException)
		{
			return aggregateException.InnerException;
		}

		return exception;
	}
	
	public static string GetPromptMessage(this Exception exception)
	{
		while (exception.InnerException != null)
		{
			exception = exception.InnerException;
		}

		return exception switch
		{
			HttpRequestException _ => "Unable to connect to the server",
			TaskCanceledException _ => "The request has timed out",
			OperationCanceledException _ => "The request has timed out",
			ApiException ex => ex.GetDetail()?.Message ?? ex.Message,
			_ => exception.Message
		};
	}

	public static void Send(this Exception exception, string token = InternalConstants.Message.ExceptionThrown)
	{
		WeakReferenceMessenger.Default.Send(exception, token);
	}
}