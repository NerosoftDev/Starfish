using Google.Protobuf;
using Microsoft.Extensions.Configuration;

namespace Nerosoft.Starfish.Etcd;
public class EtcdConfigurationProvider : ConfigurationProvider
{
	private readonly string _path;
	private readonly bool _reloadOnChange;
	private readonly EtcdClient _client;

	public EtcdConfigurationProvider(EtcdOptions options)
	{
		_client = new EtcdClient(options.Address);
		_client.Authenticate(new AuthenticateRequest { Name = options.UserName, Password = options.PassWord });
		_path = options.Path;
		_reloadOnChange = options.ReloadOnChange;
	}

	/// <summary>
	/// 重写加载方法
	/// </summary>
	public override void Load()
	{
		//读取数据
		LoadData();
		//数据发生变化是否重新加载
		if (_reloadOnChange)
		{
			ReloadData();
		}
	}

	private void LoadData()
	{
		//读取Etcd里的数据
		string result = _client.GetValAsync(_path).GetAwaiter().GetResult();
		if (string.IsNullOrEmpty(result))
		{
			return;
		}
		//转换一下数据结构，这里我使用的是json格式
		//读取的数据只要赋值到Data属性上即可,IConfiguration真正读取的数据就是存储到Data的字典数据
		Data = ConvertData(result);
	}

	private IDictionary<string, string> ConvertData(string result)
	{
		byte[] array = Encoding.UTF8.GetBytes(result);
		MemoryStream stream = new MemoryStream(array);
		//return JsonConfigurationFileParser.Parse(stream);
		return null;
	}

	private void ReloadData()
	{
		WatchRequest request = new WatchRequest()
		{
			CreateRequest = new WatchCreateRequest()
			{
				//需要转换一个格式,因为etcd v3版本的接口都包含在grpc的定义中
				Key = ByteString.CopyFromUtf8(_path)
			}
		};
		//监听Etcd节点变化,获取变更数据，更新配置
		_client.Watch(request, rsp =>
		{
			if (rsp.Events.Any())
			{
				var @event = rsp.Events[0];
				//需要转换一个格式,因为etcd v3版本的接口都包含在grpc的定义中
				Data = ConvertData(@event.Kv.Value.ToStringUtf8());
				//需要调用ConfigurationProvider的OnReload方法触发ConfigurationReloadToken通知
				//这样才能对使用Configuration的类发送数据变更通知
				//比如IOptionsMonitor就是通过ConfigurationReloadToken通知变更数据的
				OnReload();
			}
		});
	}
}