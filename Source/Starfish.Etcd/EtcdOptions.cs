namespace Nerosoft.Starfish.Etcd;

public class EtcdOptions
{
	/// <summary>
	/// Etcd地址
	/// </summary>
	public string Address { get; set; }

	/// <summary>
	/// Etcd访问用户名
	/// </summary>
	public string UserName { get; set; }

	/// <summary>
	/// Etcd访问密码
	/// </summary>
	public string PassWord { get; set; }

	/// <summary>
	/// Etcd读取路径
	/// </summary>
	public string Path { get; set; }

	/// <summary>
	/// 数据变更是否刷新读取
	/// </summary>
	public bool ReloadOnChange { get; set; }
}
