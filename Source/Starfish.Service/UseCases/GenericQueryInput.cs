using Nerosoft.Euonia.Application;

namespace Nerosoft.Starfish.UseCases;

public class GenericQueryInput<TCriteria> : IUseCaseInput
{
	public GenericQueryInput(TCriteria criteria, int skip, int count)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(skip);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);

		Criteria = criteria;
		Skip = skip;
		Count = count;
	}

	public TCriteria Criteria { get; }

	public int Skip { get; }

	public int Count { get; }
}