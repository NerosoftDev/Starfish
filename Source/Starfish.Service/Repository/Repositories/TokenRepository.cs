using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 令牌仓储
/// </summary>
public class TokenRepository : BaseRepository<DataContext, Token, int>, ITokenRepository
{
	/// <summary>
	/// 初始化<see cref="TokenRepository"/>
	/// </summary>
	/// <param name="provider"></param>
	public TokenRepository(IContextProvider provider)
		: base(provider)
	{
	}
	
	/// <summary>
	/// 根据令牌类型和令牌值查找令牌
	/// </summary>
	/// <param name="key"></param>
	/// <param name="tracking"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<Token> GetAsync(string key, bool tracking, CancellationToken cancellationToken = default)
	{
		return await GetAsync(t => t.Key == key, tracking, cancellationToken);
	}
}