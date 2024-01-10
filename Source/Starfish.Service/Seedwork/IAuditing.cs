using Nerosoft.Euonia.Domain;

namespace Nerosoft.Starfish.Service;

public interface IAuditing : IHasCreateTime, IHasUpdateTime
{
	string CreatedBy { get; set; }

	string UpdatedBy { get; set; }
}