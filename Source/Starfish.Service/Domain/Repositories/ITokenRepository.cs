using System.Linq.Expressions;
using Nerosoft.Euonia.Repository;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 令牌仓储接口
/// </summary>
public interface ITokenRepository : IRepository<Token, int>
{
	/// <summary>
	/// 跟据指定条件查询令牌
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="tracking"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<Token> GetAsync(Expression<Func<Token, bool>> predicate, bool tracking, CancellationToken cancellationToken = default);
}