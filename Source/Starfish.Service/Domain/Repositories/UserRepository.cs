using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 用户仓储
/// </summary>
public sealed class UserRepository : BaseRepository<DataContext, User, int>
{
	/// <summary>
	/// 初始化<see cref="UserRepository"/>.
	/// </summary>
	/// <param name="provider"></param>
	public UserRepository(IContextProvider provider)
		: base(provider)
	{
	}
}
