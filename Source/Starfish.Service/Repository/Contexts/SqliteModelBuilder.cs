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

			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.UserName).IsUnique();

			entity.HasMany(t => t.Roles)
			      .WithOne(t => t.User)
			      .HasForeignKey(t => t.UserId);
		});

		modelBuilder.Entity<UserRole>(entity =>
		{
			entity.ToTable("user_role");

			entity.HasKey(t => t.Id);

			entity.HasIndex([nameof(UserRole.UserId), nameof(UserRole.Name)], "IDX_USER_ROLE_UNIQUE")
			      .IsUnique();

			entity.HasOne(t => t.User)
			      .WithMany(t => t.Roles)
			      .HasForeignKey(t => t.UserId);
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
			entity.HasIndex(t => t.AppCode);
			entity.HasIndex(t => t.Environment);

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

		modelBuilder.Entity<SettingRevision>(entity =>
		{
			entity.ToTable("setting_revision");
			entity.HasKey(t => t.Id);
			entity.HasIndex(t => t.AppId);

			entity.Property(t => t.Id)
			      .IsRequired()
			      .HasValueGenerator<SnowflakeIdValueGenerator>();
		});

		modelBuilder.Entity<SettingArchive>(entity =>
		{
			entity.ToTable("setting_archive");
			entity.HasKey(t => t.Id);
			entity.HasIndex(t => t.AppId)
			      .HasDatabaseName("IDX_SETTING_ARCHIVE_APP_ID");
			entity.HasIndex([nameof(SettingArchive.AppId), nameof(SettingArchive.Environment)], "IDX_SETTING_ARCHIVE_UNIQUE")
			      .IsUnique();
			entity.HasIndex([nameof(SettingArchive.AppCode), nameof(SettingArchive.Environment)], "IDX_SETTING_ARCHIVE_COMPOSE");

			entity.Property(t => t.Id)
			      .IsRequired()
			      .HasValueGenerator<SnowflakeIdValueGenerator>();
		});
	}
}