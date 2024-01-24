using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 令牌仓储接口
/// </summary>
public interface ITokenRepository : IBaseRepository<DataContext, Token, long>
{
}