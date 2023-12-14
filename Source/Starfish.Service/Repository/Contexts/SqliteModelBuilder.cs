using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// Sqlite模型构建器
/// </summary>
public class SqliteModelBuilder : IModelBuilder
{
	/// <inheritdoc />
	public void Configure(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>(entity =>
		{
			entity.ToTable("user");
		});
		modelBuilder.Entity<Token>(entity =>
		{
			entity.ToTable("token");
		});
		modelBuilder.Entity<OperateLog>(entity =>
		{
			entity.ToTable("operate_log");
		});
		modelBuilder.Entity<AppInfo>(entity =>
		{
			entity.ToTable("app_info");
			entity.HasKey(t => t.Id);
			entity.HasIndex(t => t.Code);

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();
		});

		modelBuilder.Entity<SettingNode>(entity =>
		{
			entity.ToTable("setting_node");
			entity.HasKey(t => t.Id);
			entity.HasIndex(t => t.ParentId);
			entity.HasIndex(t => t.AppId);

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();

			entity.HasMany(t => t.Children)
				  .WithOne()
				  .HasForeignKey(t => t.ParentId)
				  .OnDelete(DeleteBehavior.Cascade);

			entity.HasOne(t => t.Parent)
				  .WithMany()
				  .HasForeignKey(t => t.ParentId)
				  .OnDelete(DeleteBehavior.Cascade);
		});
	}
}