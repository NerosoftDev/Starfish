using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Repository;

internal class PgsqlModelBuilder : IModelBuilder
{
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

		modelBuilder.Entity<Configuration>(entity =>
		{
			entity.ToTable("configuration");
			entity.HasKey(t => t.Id);
			entity.HasIndex(t => t.AppId);
			entity.HasIndex(t => t.Environment);

			entity.HasIndex([nameof(Configuration.AppId), nameof(Configuration.Environment)], "IDX_CONFIG_UNIQUE")
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
				  .HasForeignKey(t => t.ConfigurationId)
				  .OnDelete(DeleteBehavior.Cascade);

			entity.HasMany(t => t.Revisions)
				  .WithOne()
				  .HasForeignKey(t => t.ConfigurationId)
				  .OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<ConfigurationItem>(entity =>
		{
			entity.ToTable("configuration_item");
			entity.HasKey(t => t.Id);
			entity.HasIndex(t => t.ConfigurationId);

			entity.HasIndex(t => t.Key);

			entity.HasIndex([nameof(ConfigurationItem.ConfigurationId), nameof(ConfigurationItem.Key)], "IDX_CONFIG_ITEM_UNIQUE")
				  .IsUnique();

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();

			entity.HasOne(t => t.Configuration)
				  .WithMany(t => t.Items)
				  .HasForeignKey(t => t.ConfigurationId)
				  .OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<ConfigurationRevision>(entity =>
		{
			entity.ToTable("configuration_revision");
			entity.HasKey(t => t.Id);
			entity.HasIndex(t => t.ConfigurationId);

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();

			entity.HasOne(t => t.Configuration)
				  .WithMany(t => t.Revisions)
				  .HasForeignKey(t => t.ConfigurationId)
				  .OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<ConfigurationArchive>(entity =>
		{
			entity.ToTable("configuration_archive");
			entity.HasKey(t => t.Id);
			entity.HasIndex(t => t.AppId)
				  .HasDatabaseName("IDX_CONFIG_ARCHIVE_APP_ID");
			entity.HasIndex([nameof(ConfigurationArchive.AppId), nameof(ConfigurationArchive.Environment)], "IDX_CONFIG_ARCHIVE_UNIQUE")
				  .IsUnique();

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();
		});

		modelBuilder.Entity<Team>(entity =>
		{
			entity.ToTable("team");
			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.Alias).IsUnique();
			entity.HasIndex(t => t.Name);
			entity.HasIndex(t => t.OwnerId);

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