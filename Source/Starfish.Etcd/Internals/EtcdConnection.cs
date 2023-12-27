using Google.Protobuf;

namespace Nerosoft.Starfish.Etcd;

internal class EtcdConnection
{
	internal Store.StoreClient StoreClient { get; set; }

	internal Watch.WatchClient WatchClient { get; set; }

	internal Auth.AuthClient AuthClient { get; set; }
}