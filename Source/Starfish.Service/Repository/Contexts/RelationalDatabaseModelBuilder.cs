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

			entity.HasIndex(t => t.Username)
			      .HasDatabaseName("user_idx_username")
			      .IsUnique();

			entity.HasIndex(t => t.Email)
			      .HasDatabaseName("user_idx_email")
			      .IsUnique();

			entity.HasIndex(t => t.Phone)
			      .HasDatabaseName("user_idx_phone")
			      .IsUnique();

			entity.Property(t => t.Id)
			      .HasColumnName("id")
			      .IsRequired()
			      .HasValueGenerator<SequentialGuidValueGenerator>();

			entity.Property(t => t.Username)
			      .HasColumnName("username")
			      .IsRequired()
			      .HasMaxLength(64);

			entity.Property(t => t.PasswordHash)
			      .HasColumnName("password_hash")
			      .IsRequired()
			      .HasMaxLength(512);

			entity.Property(t => t.PasswordSalt)
			      .HasColumnName("password_salt")
			      .IsRequired()
			      .HasMaxLength(32);

			entity.Property(t => t.Email)
			      .HasColumnName("email")
			      .HasMaxLength(255);

			entity.Property(t => t.Phone)
			      .HasColumnName("phone")
			      .HasMaxLength(32);

			entity.Property(t => t.Nickname)
			      .HasColumnName("nickname")
			      .IsUnicode();

			entity.Property(t => t.AccessFailedCount)
			      .HasColumnName("access_failed_count");

			entity.Property(t => t.IsAdmin)
			      .HasColumnName("is_admin")
			      .HasDefaultValue(false);

			entity.Property(t => t.Reserved)
			      .HasColumnName("reserved")
			      .HasDefaultValue(false);

			entity.Property(t => t.LockoutEnd)
			      .HasColumnName("lockout_end");

			entity.Property(t => t.Source)
			      .HasColumnName("source");

			entity.Property(t => t.CreateTime)
			      .HasColumnName("create_time")
			      .HasValueGenerator<UniversalTimeValueGenerator>()
			      .ValueGeneratedOnAdd();

			entity.Property(t => t.UpdateTime)
			      .HasColumnName("update_time")
			      .HasValueGenerator<UniversalTimeValueGenerator>()
			      .ValueGeneratedOnAddOrUpdate();

			entity.Property(t => t.IsDeleted)
			      .HasColumnName("is_deleted")
			      .HasDefaultValue(false);

			entity.Property(t => t.DeleteTime)
			      .HasColumnName("delete_time");
		});
	}

	protected override ModelBuilder ConfigureTeam(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Team>(entity =>
		{
			entity.ToTable("team");
			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.Name).HasDatabaseName("team_idx_name");
			entity.HasIndex(t => t.OwnerId).HasDatabaseName("team_idx_owner_id");

			entity.Property(t => t.Id)
			      .HasColumnName("id")
			      .IsRequired()
			      .HasValueGenerator<SequentialGuidValueGenerator>()
			      .HasMaxLength(32);

			entity.Property(t => t.Name)
			      .HasColumnName("name")
			      .IsRequired()
			      .HasMaxLength(100)
			      .IsUnicode();

			entity.Property(t => t.Description)
			      .HasColumnName("description")
			      .IsRequired()
			      .HasMaxLength(2000)
			      .IsUnicode();

			entity.Property(t => t.OwnerId)
			      .HasColumnName("owner_id")
			      .IsRequired()
			      .HasMaxLength(32);

			entity.Property(t => t.MemberCount)
			      .HasColumnName("member_count")
			      .HasDefaultValue(0);

			entity.Property(t => t.CreateTime)
			      .HasColumnName("create_time")
			      .HasValueGenerator<UniversalTimeValueGenerator>()
			      .ValueGeneratedOnAdd();

			entity.Property(t => t.UpdateTime)
			      .HasColumnName("update_time")
			      .HasValueGenerator<UniversalTimeValueGenerator>()
			      .ValueGeneratedOnAddOrUpdate();

			entity.Property(t => t.CreatedBy)
			      .HasColumnName("created_by")
			      .HasMaxLength(32);

			entity.Property(t => t.UpdatedBy)
			      .HasColumnName("updated_by")
			      .HasMaxLength(32);

			entity.HasMany(t => t.Members)
			      .WithOne(t => t.Team)
			      .HasForeignKey(t => t.TeamId);
		});

		modelBuilder.Entity<TeamMember>(entity =>
		{
			entity.ToTable("team_member");
			entity.HasKey(t => t.Id);

			entity.HasIndex([nameof(TeamMember.TeamId), nameof(TeamMember.UserId)], "team_member_idx_unique")
			      .IsUnique();

			entity.Property(t => t.Id)
			      .HasColumnName("id")
			      .IsRequired()
			      .HasValueGenerator<SequentialGuidValueGenerator>();

			entity.Property(t => t.UserId)
			      .HasColumnName("user_id")
			      .IsRequired()
			      .HasMaxLength(32);

			entity.Property(t => t.TeamId)
			      .HasColumnName("team_id")
			      .IsRequired()
			      .HasMaxLength(32);

			entity.Property(t => t.CreateTime)
			      .HasColumnName("create_time")
			      .HasValueGenerator<UniversalTimeValueGenerator>()
			      .ValueGeneratedOnAdd();

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

			entity.HasIndex(t => t.TeamId).HasDatabaseName("configuration_idx_unique");
			entity.HasIndex(t => t.Status).HasDatabaseName("configuration_idx_team_id");
			entity.HasIndex(t => t.Name).HasDatabaseName("configuration_idx_name");

			entity.HasIndex([nameof(Configuration.TeamId), nameof(Configuration.Name)], "configuration_idx_unique")
			      .IsUnique();

			entity.Property(t => t.Id)
			      .HasColumnName("id")
			      .IsRequired()
			      .HasValueGenerator<SequentialGuidValueGenerator>();

			entity.Property(t => t.TeamId)
			      .HasColumnName("team_id")
			      .IsRequired()
			      .HasMaxLength(32);

			entity.Property(t => t.Name)
			      .HasColumnName("name")
			      .IsRequired()
			      .HasMaxLength(100)
			      .IsUnicode();

			entity.Property(t => t.Secret)
			      .HasColumnName("secret")
			      .IsRequired()
			      .HasMaxLength(255)
			      .IsUnicode();

			entity.Property(t => t.Version)
			      .HasColumnName("version")
			      .IsRequired()
			      .HasMaxLength(20)
			      .IsUnicode();

			entity.Property(t => t.Description)
			      .HasColumnName("description")
			      .IsRequired()
			      .HasMaxLength(2000)
			      .IsUnicode();

			entity.Property(t => t.Status)
			      .HasColumnName("status")
			      .IsRequired()
			      .HasDefaultValue(ConfigurationStatus.None);

			entity.Property(t => t.PublishTime)
			      .HasColumnName("publish_time");

			entity.Property(t => t.CreateTime)
			      .HasColumnName("create_time")
			      .HasValueGenerator<UniversalTimeValueGenerator>()
			      .ValueGeneratedOnAdd();

			entity.Property(t => t.UpdateTime)
			      .HasColumnName("update_time")
			      .HasValueGenerator<UniversalTimeValueGenerator>()
			      .ValueGeneratedOnAddOrUpdate();

			entity.Property(t => t.CreatedBy)
			      .HasColumnName("created_by")
			      .HasMaxLength(32);

			entity.Property(t => t.UpdatedBy)
			      .HasColumnName("updated_by")
			      .HasMaxLength(32);

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
			      .HasColumnName("id")
			      .IsRequired()
			      .HasValueGenerator<SequentialGuidValueGenerator>();

			entity.Property(t => t.ConfigurationId)
			      .HasColumnName("configuration_id")
			      .IsRequired()
			      .HasMaxLength(32);
			
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
			      .HasValueGenerator<SequentialGuidValueGenerator>();

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

			entity.HasIndex(t => t.Key).HasDatabaseName("token_idx_key");
			entity.HasIndex(t => t.Expires).HasDatabaseName("token_idx_expires");

			entity.Property(t => t.Id)
			      .HasColumnName("id")
			      .IsRequired()
			      .HasValueGenerator<SequentialGuidValueGenerator>();

			entity.Property(t => t.Key)
			      .HasColumnName("key")
			      .IsRequired()
			      .HasMaxLength(64);

			entity.Property(t => t.Expires)
			      .HasColumnName("expires")
			      .IsRequired();

			entity.Property(t => t.Subject)
			      .HasColumnName("subject")
			      .IsRequired()
			      .HasMaxLength(64);

			entity.Property(t => t.Issues)
			      .HasColumnName("issues")
			      .IsRequired();
		});

		modelBuilder.Entity<OperateLog>(entity =>
		{
			entity.ToTable("operate_log");

			entity.HasKey(t => t.Id);

			entity.HasIndex(t => t.Module).HasDatabaseName("operate_log_idx_module");
			entity.HasIndex(t => t.Type).HasDatabaseName("operate_log_idx_type");
			entity.HasIndex(t => t.Username).HasDatabaseName("operate_log_idx_username");

			entity.Property(t => t.Id)
			      .HasColumnName("id")
			      .IsRequired()
			      .HasValueGenerator<SequentialGuidValueGenerator>();

			entity.Property(t => t.Username)
			      .HasColumnName("username")
			      .IsRequired()
			      .HasMaxLength(64);

			entity.Property(t => t.Module)
			      .HasColumnName("module")
			      .IsRequired()
			      .HasMaxLength(64);

			entity.Property(t => t.Type)
			      .HasColumnName("type")
			      .IsRequired()
			      .HasMaxLength(64);

			entity.Property(t => t.Content)
			      .HasColumnName("content")
			      .IsUnicode();

			entity.Property(t => t.OperateTime)
			      .HasColumnName("operate_time");

			entity.Property(t => t.Error)
			      .HasColumnName("error")
			      .IsUnicode();

			entity.Property(t => t.RequestTraceId)
			      .HasColumnName("request_trace_id")
			      .HasMaxLength(64);
		});

		return modelBuilder;
	}
}