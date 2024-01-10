using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Claims;

namespace Nerosoft.Starfish.Service;

/// <summary>
/// 可编辑对象基类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class EditableObjectBase<T> : EditableObject<T>, IHasLazyServiceProvider
	where T : EditableObjectBase<T>
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