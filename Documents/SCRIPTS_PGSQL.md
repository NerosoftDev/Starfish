# app_info

```sql
CREATE TABLE "public"."appinfo" (
  "Id" int8 NOT NULL,
  "TeamId" int8 NOT NULL,
  "Name" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "Code" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "Secret" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "Description" varchar(500) COLLATE "pg_catalog"."default",
  "Status" int4 NOT NULL,
  "CreateTime" timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  "UpdateTime" timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP
)
;

CREATE INDEX "IDX_APP_INFO_CODE" ON "public"."appinfo" USING btree (
  "Code" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE INDEX "IDX_APP_INFO_STATUS" ON "public"."appinfo" USING btree (
  "Status" "pg_catalog"."int4_ops" ASC NULLS LAST
);
CREATE INDEX "IDX_APP_INFO_TEAM_ID" ON "public"."appinfo" USING btree (
  "TeamId" "pg_catalog"."int8_ops" ASC NULLS LAST
);
CREATE UNIQUE INDEX "IDX_APP_INFO_UNIQUE" ON "public"."appinfo" USING btree (
  "TeamId" "pg_catalog"."int8_ops" ASC NULLS LAST,
  "Code" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

ALTER TABLE "public"."appinfo" ADD CONSTRAINT "appinfo_pkey" PRIMARY KEY ("Id");
```

# operate_log

```sql
CREATE TABLE "public"."operate_log" (
  "Id" int8 NOT NULL,
  "Module" varchar(20) COLLATE "pg_catalog"."default" NOT NULL,
  "Type" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "Description" varchar(2000) COLLATE "pg_catalog"."default",
  "UserName" varchar(255) COLLATE "pg_catalog"."default",
  "OperateTime" timestamp(6) NOT NULL,
  "Error" varchar(2000) COLLATE "pg_catalog"."default",
  "RequestTraceId" varchar(40) COLLATE "pg_catalog"."default" NOT NULL
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

ALTER TABLE "public"."operate_log" ADD CONSTRAINT "operate_log_pkey" PRIMARY KEY ("Id");
```

# setting

```sql
CREATE TABLE "public"."setting" (
  "Id" int8 NOT NULL,
  "AppId" int8 NOT NULL,
  "Environment" varchar(25) COLLATE "pg_catalog"."default" NOT NULL,
  "Status" int4 NOT NULL,
  "Version" varchar(20) COLLATE "pg_catalog"."default",
  "PublishTime" timestamp(6),
  "CreateTime" timestamp(6) NOT NULL,
  "UpdateTime" timestamp(6) NOT NULL,
  "CreatedBy" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "UpdatedBy" date NOT NULL
)
;

CREATE INDEX "IDX_SETTING_APP_ID" ON "public"."setting" USING btree (
  "AppId" "pg_catalog"."int8_ops" ASC NULLS LAST
);
CREATE INDEX "IDX_SETTING_STATUS" ON "public"."setting" USING btree (
  "Status" "pg_catalog"."int4_ops" ASC NULLS LAST
);
CREATE UNIQUE INDEX "IDX_SETTING_UNIQUE" ON "public"."setting" USING btree (
  "AppId" "pg_catalog"."int8_ops" ASC NULLS LAST,
  "Environment" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

ALTER TABLE "public"."setting" ADD CONSTRAINT "setting_pkey" PRIMARY KEY ("Id");
```

# setting_archive

```sql
CREATE TABLE "public"."setting_archive" (
  "Id" int8 NOT NULL,
  "AppId" int8 NOT NULL,
  "Environment" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "Data" text COLLATE "pg_catalog"."default",
  "Operator" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "ArchiveTime" timestamp(6) NOT NULL
)
;

CREATE UNIQUE INDEX "IDX_SETTING_ARCHIVE_UNIQUE" ON "public"."setting_archive" USING btree (
  "AppId" "pg_catalog"."int8_ops" ASC NULLS LAST,
  "Environment" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

ALTER TABLE "public"."setting_archive" ADD CONSTRAINT "setting_archive_pkey" PRIMARY KEY ("Id");
```

# setting_item

```sql
CREATE TABLE "public"."setting_item" (
  "Id" int8 NOT NULL,
  "SettingId" int8 NOT NULL,
  "Key" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "Value" text COLLATE "pg_catalog"."default",
  "UpdatedTime" timestamp(6) NOT NULL,
  "UpdatedBy" varchar(64) COLLATE "pg_catalog"."default" NOT NULL
)
;

CREATE INDEX "IDX_SETTING_ITEM_FK" ON "public"."setting_item" USING btree (
  "SettingId" "pg_catalog"."int8_ops" ASC NULLS LAST
);
CREATE UNIQUE INDEX "IDX_SETTING_ITEM_UNIQUE" ON "public"."setting_item" USING btree (
  "SettingId" "pg_catalog"."int8_ops" ASC NULLS LAST,
  "Key" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

ALTER TABLE "public"."setting_item" ADD CONSTRAINT "setting_item_pkey" PRIMARY KEY ("Id");
```

# setting_revision

```sql
CREATE TABLE "public"."setting_revision" (
  "Id" int8 NOT NULL,
  "SettingId" int8 NOT NULL,
  "Data" text COLLATE "pg_catalog"."default",
  "Comment" varchar(1000) COLLATE "pg_catalog"."default",
  "Version" varchar(25) COLLATE "pg_catalog"."default" NOT NULL,
  "Operator" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "CreateTime" timestamp(6) NOT NULL
)
;

CREATE INDEX "IDS_SETTING_REVISION_FK" ON "public"."setting_revision" USING btree (
  "SettingId" "pg_catalog"."int8_ops" ASC NULLS LAST
);

ALTER TABLE "public"."setting_revision" ADD CONSTRAINT "setting_revision_pkey" PRIMARY KEY ("Id");
```

# team

```sql
CREATE TABLE "public"."team" (
  "Id" int8 NOT NULL,
  "Alias" varchar(100) COLLATE "pg_catalog"."default",
  "Name" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "Description" varchar(500) COLLATE "pg_catalog"."default",
  "OwnerId" int8 NOT NULL,
  "MemberCount" int4 NOT NULL DEFAULT 0,
  "CreateTime" timestamp(6) NOT NULL,
  "UpdateTime" timestamp(6) NOT NULL,
  "CreatedBy" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "UpdatedBy" varchar(64) COLLATE "pg_catalog"."default" NOT NULL
)
;

CREATE UNIQUE INDEX "IDX_TEAM_ALIAS" ON "public"."team" USING btree (
  "Alias" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE INDEX "IDX_TEAM_NAME" ON "public"."team" USING btree (
  "Name" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE INDEX "IDX_TEAM_OWNER" ON "public"."team" USING btree (
  "OwnerId" "pg_catalog"."int8_ops" ASC NULLS LAST
);

ALTER TABLE "public"."team" ADD CONSTRAINT "team_pkey" PRIMARY KEY ("Id");
```

# team_member

```sql
CREATE TABLE "public"."team_member" (
  "Id" int8 NOT NULL,
  "TeamId" int8 NOT NULL,
  "UserId" int8 NOT NULL,
  "CreateTime" timestamp(6) NOT NULL
)
;

CREATE UNIQUE INDEX "IDX_TEAM_MEMBER_UNIQUE" ON "public"."team_member" USING btree (
  "TeamId" "pg_catalog"."int8_ops" ASC NULLS LAST,
  "UserId" "pg_catalog"."int8_ops" ASC NULLS LAST
);

ALTER TABLE "public"."team_member" ADD CONSTRAINT "team_member_pkey" PRIMARY KEY ("Id");
```

# token

```sql
CREATE TABLE "public"."token" (
  "Id" int8 NOT NULL,
  "Type" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "Key" varchar(255) COLLATE "pg_catalog"."default" NOT NULL,
  "Subject" varchar(20) COLLATE "pg_catalog"."default" NOT NULL,
  "Issues" timestamp(6) NOT NULL,
  "Expires" timestamp(6)
)
;

CREATE INDEX "IDX_TOKEN_EXPIRES" ON "public"."token" USING btree (
  "Expires" "pg_catalog"."timestamp_ops" ASC NULLS LAST
);
CREATE UNIQUE INDEX "IDX_TOKEN_KEY" ON "public"."token" USING btree (
  "Key" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

ALTER TABLE "public"."token" ADD CONSTRAINT "token_pkey" PRIMARY KEY ("Id");
```

# user

```sql
CREATE TABLE "public"."user" (
  "Id" int8 NOT NULL,
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
  "CreateTime" timestamp(6) NOT NULL,
  "UpdateTime" timestamp(6) NOT NULL,
  "IsDeleted" bool NOT NULL DEFAULT false,
  "DeleteTime" timestamp(6)
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

ALTER TABLE "public"."user" ADD CONSTRAINT "user_pkey" PRIMARY KEY ("Id");
```

# user_role

```sql
CREATE TABLE "public"."user_role" (
  "Id" int8 NOT NULL,
  "UserId" int8 NOT NULL,
  "Name" varchar(100) COLLATE "pg_catalog"."default" NOT NULL
)
;

CREATE UNIQUE INDEX "IDX_USER_ROLE_UNIQUE" ON "public"."user_role" USING btree (
  "UserId" "pg_catalog"."int8_ops" ASC NULLS LAST,
  "Name" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

ALTER TABLE "public"."user_role" ADD CONSTRAINT "user_role_pkey" PRIMARY KEY ("Id");
```