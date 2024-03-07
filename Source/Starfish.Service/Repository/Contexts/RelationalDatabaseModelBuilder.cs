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
	
	protected override ModelBuilder ConfigureTeam(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Team>(entity =>
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

		return modelBuilder;
	}

	protected override ModelBuilder ConfigureConfiguration(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Configuration>(entity =>
		{
			entity.ToTable("configuration");

			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.TeamId).HasDatabaseName("IDX_CONFIG_TEAM_ID");
			entity.HasIndex(t => t.Status).HasDatabaseName("IDX_CONFIG_STATUS");
			entity.HasIndex(t => t.Name).HasDatabaseName("IDX_CONFIG_NAME");

			entity.HasIndex([nameof(Configuration.TeamId), nameof(Configuration.Name)], "IDX_CONFIG_UNIQUE")
			      .IsUnique();

			entity.Property(t => t.Id)
			      .IsRequired()
			      .HasValueGenerator<UuidValueGenerator>();

			entity.HasMany(t => t.Items)
			      .WithOne()
			      .HasForeignKey(t => t.ConfigurationId)
			      .OnDelete(DeleteBehavior.Cascade);

			entity.HasMany(t => t.Revisions)
			      .WithOne()
			      .HasForeignKey(t => t.ConfigurationId)
			      .OnDelete(DeleteBehavior.Cascade);

			entity.HasOne(t => t.Archive)
			      .WithOne()
			      .HasForeignKey<ConfigurationArchive>()
			      .OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<ConfigurationItem>(entity =>
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

		modelBuilder.Entity<ConfigurationArchive>(entity =>
		{
			entity.ToTable("configuration_archive");

			entity.HasKey(t => t.Id);

			entity.Property(t => t.Id)
			      .IsRequired();

			entity.HasOne(t => t.Configuration)
			      .WithOne(t => t.Archive)
			      .OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<ConfigurationRevision>(entity =>
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

		return modelBuilder;
	}

	protected override ModelBuilder ConfigureSupported(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Token>(entity =>
		{
			entity.ToTable("token");

			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.Key).HasDatabaseName("IDX_TOKEN_KEY");
			entity.HasIndex(t => t.Expires).HasDatabaseName("IDX_TOKEN_EXPIRES");

			entity.Property(t => t.Id)
			      .IsRequired()
			      .HasValueGenerator<SnowflakeIdValueGenerator>();
		});

		modelBuilder.Entity<OperateLog>(entity =>
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

		return modelBuilder;
	}
}