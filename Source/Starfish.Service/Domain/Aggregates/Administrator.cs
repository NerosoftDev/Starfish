using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Domain;

public class Administrator : Aggregate<long>
{
	public string UserId { get; set; }

	public User User { get; set; }

	public string Scopes { get; set; }
}