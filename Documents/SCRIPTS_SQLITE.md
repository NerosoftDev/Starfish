﻿# app_info

```sqlite
CREATE TABLE "app_info" (
    "Id" integer NOT NULL,
    "TeamId" integer NOT NULL,
    "Name" text(50) NOT NULL,
    "Code" text(50) NOT NULL,
    "Secret" text(50) NOT NULL,
    "Description" text(1000),
    "Status" integer NOT NULL DEFAULT 1,
    "CreateTime" text(20) NOT NULL,
    "UpdateTime" text(20) NOT NULL,
    PRIMARY KEY ("Id")
);

CREATE INDEX "IDX_APP_INFO_CODE"
ON "app_info" (
    "Code" ASC
);
CREATE INDEX "IDX_APP_INFO_STATUS"
ON "app_info" (
    "Status" ASC
);
```

# operate_log

```sqlite
CREATE TABLE "operate_log" (
  "Id" integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  "Module" text(20) NOT NULL,
  "Type" text(20) NOT NULL,
  "Description" text(2000),
  "UserName" text(200),
  "OperateTime" text(20) NOT NULL,
  "Error" text(2000),
  "RequestTraceId" text(40) NOT NULL
);

CREATE INDEX "IDX_OPERATE_LOG_MODULE"
ON "operate_log" (
    "Module"
);

CREATE INDEX "IDX_OPERATE_LOG_TYPE"
ON "operate_log" (
    "Type"
);

CREATE INDEX "IDX_OPERATE_LOG_USER_NAME"
ON "operate_log" (
    "UserName"
);
```

# setting

```sqlite
CREATE TABLE "setting" (
    "Id" integer NOT NULL,
    "AppId" integer NOT NULL,
    "Environment" text(20) NOT NULL,
    "Status" integer NOT NULL DEFAULT 1,
    "Version" text(20),
    "PublishTime" text(20),
    "CreateTime" text(20) NOT NULL,
    "UpdateTime" text(20) NOT NULL,
    "CreatedBy" text(200) NOT NULL,
    "UpdatedBy" text(200) NOT NULL,
    PRIMARY KEY ("Id")
);

CREATE INDEX "IDX_SETTING_APP_ID"
ON "setting" (
    "AppId" ASC
);
CREATE INDEX "IDX_SETTING_ENV"
ON "setting" (
    "Environment" ASC
);
CREATE UNIQUE INDEX "IDX_SETTING_UNIQUE"
ON "setting" (
    "AppId" ASC,
    "Environment" ASC
);
```

# setting_archive

```sqlite
CREATE TABLE "setting_archive" (
    "Id" integer NOT NULL,
    "AppId" integer NOT NULL,
    "Environment" text NOT NULL,
    "Data" text,
    "Operator" text NOT NULL,
    "ArchiveTime" text NOT NULL,
    PRIMARY KEY ("Id")
);

CREATE INDEX "IDX_SETTING_ARCHIVE_APP_ID"
ON "setting_archive" (
    "AppId" ASC
);
CREATE UNIQUE INDEX "IDX_SETTING_ARCHIVE_UNIQUE"
ON "setting_archive" (
    "AppId" ASC,
    "Environment" ASC
);
```

# setting_item

```sqlite
CREATE TABLE "setting_item" (
    "Id" integer NOT NULL,
    "SettingId" integer NOT NULL,
    "Key" text NOT NULL,
    "Value" text,
    PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IDX_SETTING_ITEM_UNIQUE"
ON "setting_item" (
    "SettingId" ASC,
    "Key" ASC
);
```

# setting_revision

```sqlite
CREATE TABLE "setting_revision" (
  "Id" integer NOT NULL,
  "SettingId" integer NOT NULL,
  "Data" text,
  "Comment" text,
  "Version" text NOT NULL,
  "Operator" text NOT NULL,
  "CreateTime" text NOT NULL,
  PRIMARY KEY ("Id")
);

CREATE INDEX "IDS_SETTING_REVISION_SETTING_ID"
ON "setting_revision" (
  "SettingId" ASC
);
```

# team

```sqlite
CREATE TABLE "team" (
  "Id" integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  "Alias" text(50),
  "Name" text(50) NOT NULL,
  "Description" text(1000),
  "OwnerId" integer NOT NULL,
  "MemberCount" integer NOT NULL DEFAULT 0,
  "CreateTime" text(20) NOT NULL,
  "UpdateTime" text(20) NOT NULL,
  "CreatedBy" text(200) NOT NULL,
  "UpdatedBy" text(200) NOT NULL
);

INSERT INTO "sqlite_sequence" (name, seq) VALUES ('team', '1000001');

CREATE INDEX "IDX_TEAM_ALIAS" ON "team" (
  "Alias" ASC
);
CREATE INDEX "IDX_TEAM_NAME" ON "team" (
  "Name" ASC
);
CREATE INDEX "IDX_TEAM_OWNER" ON "team" (
  "OwnerId" ASC
);
```

# team_member

```sqlite
CREATE TABLE "team_member" (
  "Id" integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  "TeamId" integer NOT NULL,
  "UserId" integer NOT NULL,
  "CreateTime" text NOT NULL
);

INSERT INTO "sqlite_sequence" (name, seq) VALUES ('team_member', '1000001');

CREATE UNIQUE INDEX "IDX_TEAM_MEMBER_UNIQUE" ON "team_member" (
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
```

# user

```sqlite
CREATE TABLE "user" (
    "Id" integer NOT NULL PRIMARY KEY AUTOINCREMENT,
    "UserName" text(200) NOT NULL,
    "PasswordHash" text(512) NOT NULL,
    "PasswordSalt" text(32) NOT NULL,
    "NickName" text(100),
    "Email" text(256),
    "Phone" text,
    "AccessFailedCount" integer NOT NULL DEFAULT 0,
    "LockoutEnd" text,
    "Reserved" integer NOT NULL DEFAULT 0,
    "Source" integer NOT NULL,
    "CreateTime" text(20) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdateTime" text(20) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "IsDeleted" integer NOT NULL DEFAULT 0,
    "DeleteTime" text(20)
);

INSERT INTO "sqlite_sequence" (name, seq) VALUES ('user', '1000001');

CREATE UNIQUE INDEX "IDX_USER_EMAIL"
ON "user" (
    "Email" ASC
);

CREATE UNIQUE INDEX "IDX_USER_PHONE"
ON "user" (
    "Phone"
);

CREATE UNIQUE INDEX "IDX_USER_USERNAME"
ON "user" (
    "UserName" ASC
);
```

# user_role

```sqlite
CREATE TABLE "user_role" (
    "Id" integer NOT NULL PRIMARY KEY AUTOINCREMENT,
    "UserId" integer NOT NULL,
    "Name" text(20) NOT NULL
);

INSERT INTO "sqlite_sequence" (name, seq) VALUES ('user_role', '1000001');

CREATE UNIQUE INDEX "IDX_USER_ROLE_UNIQUE"
ON "user_role" (
    "UserId" ASC,
    "Name" ASC
);
```