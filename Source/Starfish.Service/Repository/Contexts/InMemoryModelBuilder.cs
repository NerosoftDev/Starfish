using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 内存数据库模型构建器
/// </summary>
public class InMemoryModelBuilder : IModelBuilder
{
	/// <inheritdoc />
	public void Configure(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>()
		            .Property(t => t.Id);
		modelBuilder.Entity<Token>()
		            .Property(t => t.Id);
		modelBuilder.Entity<OperateLog>()
		            .Property(t => t.Id)
		            .HasValueGenerator<SnowflakeIdValueGenerator>();
	}
}