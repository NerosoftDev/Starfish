using CommunityToolkit.Mvvm.Messaging;

namespace Nerosoft.Starfish.Webapp;

internal sealed class ExceptionRecipient : IDisposable
{
	public ExceptionRecipient()
	{
		WeakReferenceMessenger.Default.Unregister<Exception, string>(this, Constants.Message.ExceptionThrown);
		WeakReferenceMessenger.Default.Register<Exception, string>(this, Constants.Message.ExceptionThrown, OnExceptionThrown);
	}

	private Action<Exception> Handle { get; set; }

	public void Dispose()
	{
		Handle = null;
		WeakReferenceMessenger.Default.Unregister<Exception, string>(this, Constants.Message.ExceptionThrown);
	}

	public void Subscribe(Action<Exception> handle)
	{
		Handle = handle;
	}

	private void OnExceptionThrown(object recipient, Exception exception)
	{
		Handle?.Invoke(exception);
	}
}