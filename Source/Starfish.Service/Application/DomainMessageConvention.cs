using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Application;

/// <inheritdoc />
public class DomainMessageConvention : IMessageConvention
{
	/// <inheritdoc />
	public bool IsQueueType(Type type)
	{
		return type.IsAssignableTo(typeof(ICommand));
	}

	/// <inheritdoc />
	public bool IsTopicType(Type type)
	{
		return type.IsAssignableTo(typeof(IEvent));
	}

	/// <inheritdoc />
	public string Name => nameof(DomainMessageConvention);
}