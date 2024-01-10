using Microsoft.Extensions.Configuration;

namespace Nerosoft.Starfish.Etcd;

public class EtcdConfigurationSource : IConfigurationSource
{
	public EtcdOptions EtcdOptions { get; set; }

	public IConfigurationProvider Build(IConfigurationBuilder builder)
	{
		return new EtcdConfigurationProvider(EtcdOptions);
	}
}
