using Microsoft.EntityFrameworkCore;
using Nerosoft.Euonia.Repository.EfCore;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

internal abstract class RelationalDatabaseModelBuilder : AbstractDatabaseModelBuilder
{
	protected override ModelBuilder ConfigureUser(ModelBuilder modelBuilder)
	{
		return modelBuilder.Entity<User>(entity =>
		{
			entity.ToTable("user");

			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.UserName)
			.HasDatabaseName("IDX_USER_NAME")
				  .IsUnique();

			entity.HasIndex(t => t.Email)
				  .HasDatabaseName("IDX_USER_EMAIL")
				  .IsUnique();

			entity.HasIndex(t => t.Phone)
				  .HasDatabaseName("IDX_USER_PHONE")
				  .IsUnique();

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<UuidValueGenerator>();
		});
	}

	protected override ModelBuilder ConfigureAdministrator(ModelBuilder modelBuilder)
	{
		return modelBuilder.Entity<Administrator>(entity =>
		{
			entity.ToTable("administrator");

			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.UserId).HasDatabaseName("IDX_ADMIN_USER_ID")
				  .IsUnique();

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();

			entity.HasOne(t => t.User)
				  .WithMany()
				  .HasForeignKey(t => t.UserId);
		});
	}

	protected override ModelBuilder ConfigureTeam(ModelBuilder modelBuilder)
	{
		return modelBuilder.Entity<Team>(entity =>
		{
			entity.ToTable("team");
			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.Name).HasDatabaseName("IDX_TEAM_NAME");
			entity.HasIndex(t => t.OwnerId).HasDatabaseName("IDX_TEAM_OWNER");

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<UuidValueGenerator>();

			entity.HasMany(t => t.Members)
				  .WithOne(t => t.Team)
				  .HasForeignKey(t => t.TeamId);
		});
	}

	protected override ModelBuilder ConfigureTeamMember(ModelBuilder modelBuilder)
	{
		return modelBuilder.Entity<TeamMember>(entity =>
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

	protected override ModelBuilder ConfigureApplication(ModelBuilder modelBuilder)
	{
		return modelBuilder.Entity<AppInfo>(entity =>
		{
			entity.ToTable("app_info");
			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.TeamId).HasDatabaseName("IDX_APP_TEAM_ID");
			entity.HasIndex(t => t.Status).HasDatabaseName("IDX_APP_INFO_TEAM_ID");

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<UuidValueGenerator>();
		});
	}

	protected override ModelBuilder ConfigureConfiguration(ModelBuilder modelBuilder)
	{
		return modelBuilder.Entity<Configuration>(entity =>
		{
			entity.ToTable("configuration");

			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.AppId).HasDatabaseName("IDX_CONFIG_APP_ID");
			entity.HasIndex(t => t.Status).HasDatabaseName("IDX_CONFIG_STATUS");

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
	}

	protected override ModelBuilder ConfigureConfigurationItem(ModelBuilder modelBuilder)
	{
		return modelBuilder.Entity<ConfigurationItem>(entity =>
		{
			entity.ToTable("configuration_item");

			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.ConfigurationId).HasDatabaseName("IDX_CONFIG_ITEM_FK");
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
	}

	protected override ModelBuilder ConfigureConfigurationArchive(ModelBuilder modelBuilder)
	{
		return modelBuilder.Entity<ConfigurationArchive>(entity =>
		{
			entity.ToTable("configuration_archive");

			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.AppId).HasDatabaseName("IDX_CONFIG_ARCHIVE_APP_ID");
			entity.HasIndex([nameof(ConfigurationArchive.AppId), nameof(ConfigurationArchive.Environment)], "IDX_CONFIG_ARCHIVE_UNIQUE")
				  .IsUnique();

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();
		});
	}

	protected override ModelBuilder ConfigureConfigurationRevision(ModelBuilder modelBuilder)
	{
		return modelBuilder.Entity<ConfigurationRevision>(entity =>
		{
			entity.ToTable("configuration_revision");

			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.ConfigurationId).HasDatabaseName("IDS_CONFIG_REVISION_FK");

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();

			entity.HasOne(t => t.Configuration)
				  .WithMany(t => t.Revisions)
				  .HasForeignKey(t => t.ConfigurationId)
				  .OnDelete(DeleteBehavior.Cascade);
		});
	}

	protected override ModelBuilder ConfigureToken(ModelBuilder modelBuilder)
	{
		return modelBuilder.Entity<Token>(entity =>
		{
			entity.ToTable("token");

			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.Key).HasDatabaseName("IDX_TOKEN_KEY");
			entity.HasIndex(t => t.Expires).HasDatabaseName("IDX_TOKEN_EXPIRES");

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();
		});
	}

	protected override ModelBuilder ConfigureOperationLog(ModelBuilder modelBuilder)
	{
		return modelBuilder.Entity<OperateLog>(entity =>
		{
			entity.ToTable("operate_log");

			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.Module).HasDatabaseName("IDX_OPERATE_LOG_MODULE");
			entity.HasIndex(t => t.Type).HasDatabaseName("IDX_OPERATE_LOG_TYPE");
			entity.HasIndex(t => t.UserName).HasDatabaseName("IDX_OPERATE_LOG_USER_NAME");

			entity.Property(t => t.Id)
				  .IsRequired()
				  .HasValueGenerator<SnowflakeIdValueGenerator>();
		});
	}
}