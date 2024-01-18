namespace Nerosoft.Starfish.Client;

internal class GrpcConfigurationClient : IConfigurationClient
{
	public Task GetConfigurationAsync(Action<byte[], int> dataAction, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
}