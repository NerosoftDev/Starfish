using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Claims;

namespace Nerosoft.Starfish.Service;

public abstract class CommandObjectBase<T> : CommandObject<T>, IHasLazyServiceProvider
	where T : CommandObjectBase<T>
{
	/// <summary>
	/// 懒加载服务提供程序
	/// </summary>
	public ILazyServiceProvider LazyServiceProvider { get; set; }

	/// <summary>
	/// 当前授权用户信息
	/// </summary>
	protected virtual UserPrincipal Identity => LazyServiceProvider.GetRequiredService<UserPrincipal>();
}