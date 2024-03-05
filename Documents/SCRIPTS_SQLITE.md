# operate_log

```sqlite
CREATE TABLE "operate_log" (
  "Id" integer NOT NULL,
  "Module" text NOT NULL,
  "Type" text NOT NULL,
  "Description" text,
  "UserName" text,
  "OperateTime" text NOT NULL,
  "Error" text,
  "RequestTraceId" text NOT NULL,
  PRIMARY KEY ("Id")
);

CREATE INDEX "IDX_OPERATE_LOG_MODULE"
ON "operate_log" (
  "Module" ASC
);
CREATE INDEX "IDX_OPERATE_LOG_TYPE"
ON "operate_log" (
  "Type" ASC
);
CREATE INDEX "IDX_OPERATE_LOG_USER_NAME"
ON "operate_log" (
  "UserName" ASC
);
```

# configuration

```sqlite
CREATE TABLE "configuration" (
  "Id" text(32) NOT NULL,
  "TeamId" text(32) NOT NULL,
  "Name" text NOT NULL,
  "Secret" text(50) NOT NULL,
  "Description" text(500),
  "Status" integer NOT NULL DEFAULT 1,
  "Version" text,
  "PublishTime" text,
  "CreateTime" text NOT NULL,
  "UpdateTime" text NOT NULL,
  "CreatedBy" text NOT NULL,
  "UpdatedBy" text NOT NULL,
  PRIMARY KEY ("Id")
);

CREATE INDEX "IDX_CONFIG_TEAM_ID"
ON "configuration" (
  "TeamId" ASC
);
CREATE INDEX "IDX_CONFIG_STATUS"
ON "configuration" (
  "Status" ASC
);
CREATE UNIQUE INDEX "IDX_CONFIG_UNIQUE"
ON "configuration" (
  "TeamId" ASC,
  "Name" ASC
);
```

# configuration_archive

```sqlite
CREATE TABLE "configuration_archive" (
  "Id" text(32) NOT NULL,
  "Data" text,
  "Operator" text NOT NULL,
  "ArchiveTime" text NOT NULL,
  PRIMARY KEY ("Id")
);
```

# configuration_item

```sqlite
CREATE TABLE "configuration_item" (
  "Id" integer NOT NULL,
  "ConfigurationId" text(32) NOT NULL,
  "Key" text NOT NULL,
  "Value" text,
  "UpdateTime" text NOT NULL,
  "UpdatedBy" text(64) NOT NULL,
  PRIMARY KEY ("Id")
);

CREATE INDEX "IDX_CONFIG_ITEM_FK"
ON "configuration_item" (
  "ConfigurationId" ASC
);
CREATE UNIQUE INDEX "IDX_CONFIG_ITEM_UNIQUE"
ON "configuration_item" (
  "ConfigurationId" ASC,
  "Key" ASC
);
```

# configuration_revision

```sqlite
CREATE TABLE "configuration_revision" (
  "Id" integer NOT NULL,
  "ConfigurationId" text(32) NOT NULL,
  "Data" text,
  "Comment" text,
  "Version" text NOT NULL,
  "Operator" text NOT NULL,
  "CreateTime" text NOT NULL,
  PRIMARY KEY ("Id")
);

CREATE INDEX "IDS_CONFIG_REVISION_FK"
ON "configuration_revision" (
  "ConfigurationId" ASC
);
```

# team

```sqlite
CREATE TABLE "team" (
  "Id" text(32) NOT NULL,
  "Name" text NOT NULL,
  "Description" text,
  "OwnerId" text(32) NOT NULL,
  "MemberCount" integer NOT NULL DEFAULT 0,
  "CreateTime" text NOT NULL,
  "UpdateTime" text NOT NULL,
  "CreatedBy" text NOT NULL,
  "UpdatedBy" text NOT NULL,
  PRIMARY KEY ("Id")
);

CREATE INDEX "IDX_TEAM_NAME"
ON "team" (
  "Name" ASC
);
CREATE INDEX "IDX_TEAM_OWNER"
ON "team" (
  "OwnerId" ASC
);
```

# team_member

```sqlite
CREATE TABLE "team_member" (
  "Id" integer NOT NULL,
  "TeamId" text(32) NOT NULL,
  "UserId" text(32) NOT NULL,
  "CreateTime" text NOT NULL,
  PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IDX_TEAM_MEMBER_UNIQUE"
ON "team_member" (
  "TeamId" ASC,
  "UserId" ASC
);
```

# token

```sqlite
CREATE TABLE "token" (
  "Id" integer NOT NULL,
  "Type" text(32) NOT NULL,
  "Key" text(256) NOT NULL,
  "Subject" text(20) NOT NULL,
  "Issues" text NOT NULL,
  "Expires" text,
  PRIMARY KEY ("Id")
);

CREATE INDEX "IDX_TOKEN_EXPIRES"
ON "token" (
  "Expires" ASC
);
CREATE UNIQUE INDEX "IDX_TOKEN_KEY"
ON "token" (
  "Key" ASC
);
```

# user

```sqlite
CREATE TABLE "user" (
  "Id" text(32) NOT NULL,
  "UserName" text(64) NOT NULL,
  "PasswordHash" text(512) NOT NULL,
  "PasswordSalt" text(32) NOT NULL,
  "NickName" text(100),
  "Email" text(256),
  "Phone" text(25),
  "AccessFailedCount" integer NOT NULL DEFAULT 0,
  "LockoutEnd" text,
  "Reserved" integer NOT NULL DEFAULT 0,
  "IsAdmin" integer NOT NULL DEFAULT 0,
  "Source" integer NOT NULL,
  "CreateTime" text NOT NULL DEFAULT CURRENT_TIMESTAMP,
  "UpdateTime" text NOT NULL DEFAULT CURRENT_TIMESTAMP,
  "IsDeleted" integer NOT NULL DEFAULT 0,
  "DeleteTime" text,
  PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IDX_USER_EMAIL"
ON "user" (
  "Email" ASC
);
CREATE UNIQUE INDEX "IDX_USER_PHONE"
ON "user" (
  "Phone" ASC
);
CREATE UNIQUE INDEX "IDX_USER_USERNAME"
ON "user" (
  "UserName" ASC
);
```