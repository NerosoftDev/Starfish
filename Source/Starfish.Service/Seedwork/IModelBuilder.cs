using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Service;

public interface IModelBuilder
{
	void Configure(ModelBuilder modelBuilder);
}