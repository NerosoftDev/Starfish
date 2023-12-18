using Microsoft.EntityFrameworkCore;

namespace Nerosoft.Starfish.Service;

/// <summary>
/// 数据模型构建
/// </summary>
public interface IModelBuilder
{
	/// <summary>
	/// 配置<paramref name="modelBuilder"/>
	/// </summary>
	/// <param name="modelBuilder"></param>
	void Configure(ModelBuilder modelBuilder);
}