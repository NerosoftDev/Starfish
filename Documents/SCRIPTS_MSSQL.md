# operate_log

```sql
CREATE TABLE [operate_log] (
  [Id] bigint  NOT NULL,
  [Module] varchar(20) NOT NULL,
  [Type] varchar(50) NOT NULL,
  [Content] varchar(2000) NULL,
  [UserName] varchar(64) NULL,
  [OperateTime] datetime DEFAULT getdate() NOT NULL,
  [Error] varchar(2000) NULL,
  [RequestTraceId] varchar(40) NOT NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO

ALTER TABLE [operate_log] SET (LOCK_ESCALATION = TABLE)
GO

CREATE NONCLUSTERED INDEX [IDX_OPERATE_LOG_MODULE]
ON [operate_log] (
  [Module] ASC
)
GO

CREATE NONCLUSTERED INDEX [IDX_OPERATE_LOG_TYPE]
ON [operate_log] (
  [Type] ASC
)
GO

CREATE NONCLUSTERED INDEX [IDX_OPERATE_LOG_USER_NAME]
ON [operate_log] (
  [UserName] ASC
)
GO
```

# configuration

```sql
CREATE TABLE [configuration] (
  [Id] varchar(32) NOT NULL,
  [TeamId] varchar(32) NOT NULL,
  [Name] varchar(100) NOT NULL,
  [Secret] varchar(255) NOT NULL,
  [Description] varchar(500) NULL,
  [Status] int  NOT NULL,
  [Version] varchar(20) NULL,
  [PublishTime] datetime  NULL,
  [CreateTime] datetime  NOT NULL,
  [UpdateTime] datetime  NOT NULL,
  [CreatedBy] varchar(64) NOT NULL,
  [UpdatedBy] varchar(64) NOT NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO

ALTER TABLE [configuration] SET (LOCK_ESCALATION = TABLE)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IDX_CONFIG_UNIQUE]
ON [configuration] (
  [TeamId] ASC,
  [Name] ASC
)
GO

CREATE NONCLUSTERED INDEX [IDX_CONFIG_TEAM_ID]
ON [configuration] (
  [TeamId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IDX_CONFIG_STATUS]
ON [configuration] (
  [Status] ASC
)
GO
```

# configuration_archive

```sql
CREATE TABLE [configuration_archive] (
  [Id] varchar(32) NOT NULL,
  [Data] text NULL,
  [Operator] varchar(64) NOT NULL,
  [ArchiveTime] datetime  NOT NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO
```

# configuration_item

```sql
CREATE TABLE [configuration_item] (
  [Id] bigint NOT NULL,
  [ConfigurationId] varchar(32) NOT NULL,
  [Key] varchar(255) NOT NULL,
  [Value] text NULL,
  [UpdateTime] datetime DEFAULT getdate() NOT NULL,
  [UpdatedBy] varchar(64) NOT NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO

ALTER TABLE [configuration_item] SET (LOCK_ESCALATION = TABLE)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IDX_CONFIG_ITEM_UNIQUE]
ON [configuration_item] (
  [ConfigurationId] ASC,
  [Key] ASC
)
GO

CREATE NONCLUSTERED INDEX [IDX_CONFIG_ITEM_FK]
ON [configuration_item] (
  [ConfigurationId] ASC
)
GO
```

# configuration_revision

```sql
CREATE TABLE [configuration_revision] (
  [Id] bigint NOT NULL,
  [ConfigurationId] varchar(32) NOT NULL,
  [Data] text NULL,
  [Comment] varchar(1000) NULL,
  [Version] varchar(25) NOT NULL,
  [Operator] varchar(64) NOT NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO

ALTER TABLE [configuration_revision] SET (LOCK_ESCALATION = TABLE)
GO

CREATE NONCLUSTERED INDEX [IDS_CONFIG_REVISION_FK]
ON [configuration_revision] (
  [ConfigurationId] ASC
)
GO
```

# team

```sql
CREATE TABLE [team] (
  [Id] varchar(32)  NOT NULL,
  [Name] varchar(100) NOT NULL,
  [Description] varchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT NULL NULL,
  [OwnerId] varchar(32)  NOT NULL,
  [MemberCount] int DEFAULT 0 NOT NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL,
  [UpdateTime] datetime DEFAULT getdate() NOT NULL,
  [CreatedBy] varchar(64) NOT NULL,
  [UpdatedBy] varchar(64) NOT NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO

ALTER TABLE [team] SET (LOCK_ESCALATION = TABLE)
GO

CREATE NONCLUSTERED INDEX [IDX_TEAM_NAME]
ON [team] (
  [Name] ASC
)
GO

CREATE NONCLUSTERED INDEX [IDX_TEAM_OWNER]
ON [team] (
  [OwnerId] ASC
)
GO
```

# team_member

```sql
CREATE TABLE [team_member] (
  [Id] bigint  NOT NULL,
  [TeamId] varchar(32)  NOT NULL,
  [UserId] varchar(32)  NOT NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO

ALTER TABLE [team_member] SET (LOCK_ESCALATION = TABLE)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IDX_TEAM_MEMBER_UNIQUE]
ON [team_member] (
  [TeamId] ASC,
  [UserId] ASC
)
GO
```

# token

```sql
CREATE TABLE [token] (
  [Id] bigint  NOT NULL,
  [Type] varchar(32) NOT NULL,
  [Key] varchar(255) NOT NULL,
  [Subject] varchar(20) NOT NULL,
  [Issues] datetime  NOT NULL,
  [Expires] datetime DEFAULT NULL NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO

ALTER TABLE [token] SET (LOCK_ESCALATION = TABLE)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IDX_TOKEN_KEY]
ON [token] (
  [Key] ASC
)
GO

CREATE NONCLUSTERED INDEX [IDX_TOKEN_EXPIRES]
ON [token] (
  [Expires] ASC
)
GO
```

# user

```sql
CREATE TABLE [user] (
  [Id] varchar(32)  NOT NULL,
  [UserName] varchar(64) NOT NULL,
  [PasswordHash] varchar(512) NOT NULL,
  [PasswordSalt] varchar(32) NOT NULL,
  [NickName] varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT NULL NULL,
  [Email] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT NULL NULL,
  [Phone] varchar(25) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT NULL NULL,
  [AccessFailedCount] int  NOT NULL,
  [LockoutEnd] datetime DEFAULT NULL NULL,
  [Reserved] bit DEFAULT 0 NOT NULL,
  [IsAdmin] bit DEFAULT 0 NOT NULL,
  [Source] int  NOT NULL,
  [CreateTime] datetime getdate() NOT NULL,
  [UpdateTime] datetime getdate() NOT NULL,
  [IsDeleted] bit DEFAULT 0 NOT NULL,
  [DeleteTime] datetime DEFAULT NULL NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO

ALTER TABLE [user] SET (LOCK_ESCALATION = TABLE)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IDX_USER_USERNAME]
ON [user] (
  [UserName] ASC
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IDX_USER_EMAIL]
ON [user] (
  [Email] ASC
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IDX_USER_PHONE]
ON [user] (
  [Phone] ASC
)
GO
```