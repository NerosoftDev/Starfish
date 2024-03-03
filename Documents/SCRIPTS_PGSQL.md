# operate_log

```sql
CREATE TABLE "public"."operate_log" (
  "Id" int8 NOT NULL,
  "Module" varchar(20) COLLATE "pg_catalog"."default" NOT NULL,
  "Type" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "Description" varchar(2000) COLLATE "pg_catalog"."default",
  "UserName" varchar(255) COLLATE "pg_catalog"."default",
  "OperateTime" timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  "Error" varchar(2000) COLLATE "pg_catalog"."default",
  "RequestTraceId" varchar(40) COLLATE "pg_catalog"."default" NOT NULL,
  PRIMARY KEY ("Id")
)
;

CREATE INDEX "IDX_OPERATE_LOG_MODULE" ON "public"."operate_log" USING btree (
  "Module" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE INDEX "IDX_OPERATE_LOG_TYPE" ON "public"."operate_log" USING btree (
  "Type" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE INDEX "IDX_OPERATE_LOG_USER_NAME" ON "public"."operate_log" USING btree (
  "UserName" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
```

# configuration

```sql
CREATE TABLE "public"."configuration" (
  "Id" int8 NOT NULL,
  "TeamId" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "Name" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "Secret" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "Description" varchar(500) COLLATE "pg_catalog"."default",
  "Status" int4 NOT NULL,
  "Version" varchar(20) COLLATE "pg_catalog"."default",
  "PublishTime" timestamp(6),
  "CreateTime" timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  "UpdateTime" timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  "CreatedBy" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "UpdatedBy" date NOT NULL,
  PRIMARY KEY ("Id")
);

CREATE INDEX "IDX_CONFIG_TEAM_ID" ON "public"."configuration" USING btree (
  "TeamId" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE INDEX "IDX_CONFIG_STATUS" ON "public"."configuration" USING btree (
  "Status" "pg_catalog"."int4_ops" ASC NULLS LAST
);
CREATE UNIQUE INDEX "IDX_CONFIG_UNIQUE" ON "public"."configuration" USING btree (
  "TeamId" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST,
  "Name" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
```

# configuration_archive

```sql
CREATE TABLE "public"."configuration_archive" (
  "Id" int8 NOT NULL,
  "AppId" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "Environment" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "Data" text COLLATE "pg_catalog"."default",
  "Operator" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "ArchiveTime" timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY ("Id")
)
;

CREATE UNIQUE INDEX "IDX_CONFIG_ARCHIVE_UNIQUE" ON "public"."configuration_archive" USING btree (
  "AppId" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST,
  "Environment" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
```

# configuration_item

```sql
CREATE TABLE "public"."configuration_item" (
  "Id" int8 NOT NULL,
  "ConfigurationId" int8 NOT NULL,
  "Key" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "Value" text COLLATE "pg_catalog"."default",
  "UpdatedTime" timestamp(6) NOT NULL,
  "UpdatedBy" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  PRIMARY KEY ("Id")
)
;

CREATE INDEX "IDX_CONFIG_ITEM_FK" ON "public"."configuration_item" USING btree (
  "ConfigurationId" "pg_catalog"."int8_ops" ASC NULLS LAST
);
CREATE UNIQUE INDEX "IDX_CONFIG_ITEM_UNIQUE" ON "public"."configuration_item" USING btree (
  "ConfigurationId" "pg_catalog"."int8_ops" ASC NULLS LAST,
  "Key" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
```

# configuration_revision

```sql
CREATE TABLE "public"."configuration_revision" (
  "Id" int8 NOT NULL,
  "ConfigurationId" int8 NOT NULL,
  "Data" text COLLATE "pg_catalog"."default",
  "Comment" varchar(1000) COLLATE "pg_catalog"."default",
  "Version" varchar(25) COLLATE "pg_catalog"."default" NOT NULL,
  "Operator" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "CreateTime" timestamp(6) NOT NULL,
  PRIMARY KEY ("Id")
)
;

CREATE INDEX "IDS_CONFIG_REVISION_FK" ON "public"."configuration_revision" USING btree (
  "ConfigurationId" "pg_catalog"."int8_ops" ASC NULLS LAST
);
```

# team

```sql
CREATE TABLE "public"."team" (
  "Id" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "Name" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "Description" varchar(500) COLLATE "pg_catalog"."default",
  "OwnerId" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "MemberCount" int4 NOT NULL DEFAULT 0,
  "CreateTime" timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  "UpdateTime" timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  "CreatedBy" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "UpdatedBy" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  PRIMARY KEY ("Id")
)
;

CREATE INDEX "IDX_TEAM_NAME" ON "public"."team" USING btree (
  "Name" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE INDEX "IDX_TEAM_OWNER" ON "public"."team" USING btree (
  "OwnerId" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
```

# team_member

```sql
CREATE TABLE "public"."team_member" (
  "Id" int8 NOT NULL,
  "TeamId" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "UserId" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "CreateTime" timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY ("Id")
)
;

CREATE UNIQUE INDEX "IDX_TEAM_MEMBER_UNIQUE" ON "public"."team_member" USING btree (
  "TeamId" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST,
  "UserId" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
```

# token

```sql
CREATE TABLE "public"."token" (
  "Id" int8 NOT NULL,
  "Type" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "Key" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "Subject" varchar(20) COLLATE "pg_catalog"."default" NOT NULL,
  "Issues" timestamp(6) NOT NULL,
  "Expires" timestamp(6),
  PRIMARY KEY ("Id")
)
;

CREATE INDEX "IDX_TOKEN_EXPIRES" ON "public"."token" USING btree (
  "Expires" "pg_catalog"."timestamp_ops" ASC NULLS LAST
);
CREATE UNIQUE INDEX "IDX_TOKEN_KEY" ON "public"."token" USING btree (
  "Key" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
```

# user

```sql
CREATE TABLE "public"."user" (
  "Id" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "UserName" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "PasswordHash" varchar(512) COLLATE "pg_catalog"."default" NOT NULL,
  "PasswordSalt" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "NickName" varchar(100) COLLATE "pg_catalog"."default",
  "Email" varchar(255) COLLATE "pg_catalog"."default",
  "Phone" varchar(25) COLLATE "pg_catalog"."default",
  "AccessFailedCount" int4 NOT NULL DEFAULT 0,
  "LockoutEnd" timestamp(6),
  "Reserved" bool NOT NULL DEFAULT false,
  "Source" int4 NOT NULL,
  "CreateTime" timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  "UpdateTime" timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  "IsDeleted" bool NOT NULL DEFAULT false,
  "DeleteTime" timestamp(6),
  PRIMARY KEY ("Id")
)
;

CREATE INDEX "IDX_USER_EMAIL" ON "public"."user" USING btree (
  "Email" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE INDEX "IDX_USER_PHONE" ON "public"."user" USING btree (
  "Phone" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE INDEX "IDX_USER_USERNAME" ON "public"."user" USING btree (
  "UserName" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
```

# user_role

```sql
CREATE TABLE "public"."user_role" (
  "Id" int8 NOT NULL,
  "UserId" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "Name" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  PRIMARY KEY ("Id")
)
;

CREATE UNIQUE INDEX "IDX_USER_ROLE_UNIQUE" ON "public"."user_role" USING btree (
  "UserId" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST,
  "Name" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
```