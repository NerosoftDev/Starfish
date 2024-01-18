namespace Nerosoft.Starfish.Client;

/// <summary>
/// 客户端接口
/// </summary>
public interface IConfigurationClient
{
	/// <summary>
	/// 获取配置数据
	/// </summary>
	/// <param name="dataAction"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task GetConfigurationAsync(Action<byte[], int> dataAction, CancellationToken cancellationToken = default);
}