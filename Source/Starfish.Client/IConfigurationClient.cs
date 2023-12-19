namespace Nerosoft.Starfish.Client;

/// <summary>
/// 客户端接口
/// </summary>
public interface IConfigurationClient
{
	Task GetConfigurationAsync(Action<byte[], int> dataAction, CancellationToken cancellationToken = default);
}