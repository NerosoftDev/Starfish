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
		modelBuilder.Entity<User>(entity =>
		{
			entity.ToTable("user");

			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.UserName)
				  .IsUnique();

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();

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

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();

			entity.HasOne(t => t.User)
				  .WithMany(t => t.Roles)
				  .HasForeignKey(t => t.UserId);
		});

		modelBuilder.Entity<Token>(entity =>
		{
			entity.ToTable("token");

			entity.HasKey(t => t.Id);

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();
		});
		modelBuilder.Entity<OperateLog>(entity =>
		{
			entity.ToTable("operate_log");

			entity.HasKey(t => t.Id);

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();
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

		modelBuilder.Entity<Setting>(entity =>
		{
			entity.ToTable("setting");
			entity.HasKey(t => t.Id);
			entity.HasIndex(t => t.AppId);
			entity.HasIndex(t => t.Environment);

			entity.HasIndex([nameof(Setting.AppId), nameof(Setting.Environment)], "IDX_SETTING_UNIQUE")
				  .IsUnique();

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();

			entity.HasOne(t => t.App)
				  .WithMany()
				  .HasForeignKey(t => t.AppId)
				  .OnDelete(DeleteBehavior.Cascade);

			entity.HasMany(t => t.Items)
				  .WithOne()
				  .HasForeignKey(t => t.SettingId)
				  .OnDelete(DeleteBehavior.Cascade);

			entity.HasMany(t => t.Revisions)
				  .WithOne()
				  .HasForeignKey(t => t.SettingId)
				  .OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<SettingItem>(entity =>
		{
			entity.ToTable("setting_item");
			entity.HasKey(t => t.Id);
			entity.HasIndex(t => t.SettingId);

			entity.HasIndex(t => t.Key);

			entity.HasIndex([nameof(SettingItem.SettingId), nameof(SettingItem.Key)], "IDX_SETTING_ITEM_UNIQUE")
				  .IsUnique();

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();

			entity.HasOne(t => t.Setting)
				  .WithMany(t => t.Items)
				  .HasForeignKey(t => t.SettingId)
				  .OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<SettingRevision>(entity =>
		{
			entity.ToTable("setting_revision");
			entity.HasKey(t => t.Id);
			entity.HasIndex(t => t.SettingId);

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();

			entity.HasOne(t => t.Setting)
				  .WithMany(t => t.Revisions)
				  .HasForeignKey(t => t.SettingId)
				  .OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<SettingArchive>(entity =>
		{
			entity.ToTable("setting_archive");
			entity.HasKey(t => t.Id);
			entity.HasIndex(t => t.AppId)
				  .HasDatabaseName("IDX_SETTING_ARCHIVE_APP_ID");
			entity.HasIndex([nameof(SettingArchive.AppId), nameof(SettingArchive.Environment)], "IDX_SETTING_ARCHIVE_UNIQUE")
				  .IsUnique();

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();
		});

		modelBuilder.Entity<Team>(entity =>
		{
			entity.ToTable("team");
			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.Alias).HasDatabaseName("IDX_TEAM_ALIAS").IsUnique();
			entity.HasIndex(t => t.Name).HasDatabaseName("IDX_TEAM_NAME");
			entity.HasIndex(t => t.OwnerId).HasDatabaseName("IDX_TEAM_OWNER");

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();

			entity.HasMany(t => t.Members)
				  .WithOne(t => t.Team)
				  .HasForeignKey(t => t.TeamId);
		});

		modelBuilder.Entity<TeamMember>(entity =>
		{
			entity.ToTable("team_member");
			entity.HasKey(t => t.Id);

			entity.HasIndex([nameof(TeamMember.TeamId), nameof(TeamMember.UserId)], "IDX_TEAM_MEMBER_UNIQUE")
				  .IsUnique();

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();

			entity.HasOne(t => t.Team)
				  .WithMany(t => t.Members)
				  .HasForeignKey(t => t.TeamId);

			entity.HasOne(t => t.User)
				  .WithMany()
				  .HasForeignKey(t => t.UserId);
		});
	}
}