# operate_log

```sql
CREATE TABLE `operate_log`  (
  `id` bigint NOT NULL,
  `module` varchar(20) NOT NULL,
  `type` varchar(50) NOT NULL,
  `content` varchar(2000) NULL DEFAULT NULL,
  `username` varchar(64) NULL DEFAULT NULL,
  `operate_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `error` varchar(2000) NULL DEFAULT NULL,
  `request_trace_id` varchar(40) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `operate_log_idx_module`(`module` ASC) USING BTREE,
  INDEX `operate_log_idx_type`(`type` ASC) USING BTREE,
  INDEX `operate_log_idx_username`(`username` ASC) USING BTREE
);
```

# configuration

```sql
CREATE TABLE `configuration`  (
  `id` bigint NOT NULL,
  `team_id` varchar(32) NOT NULL,
  `name` varchar(100) NOT NULL,
  `secret` varchar(255) NOT NULL,
  `description` varchar(500) NULL DEFAULT NULL,
  `status` int NOT NULL,
  `version` varchar(20) NULL DEFAULT NULL,
  `publish_time` datetime NULL DEFAULT NULL,
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(64) NOT NULL,
  `updated_by` varchar(64) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `configuration_idx_unique`(`team_id` ASC, `name` ASC) USING BTREE,
  INDEX `configuration_idx_team_id`(`team_id` ASC) USING BTREE,
  INDEX `configuration_idx_status`(`status` ASC) USING BTREE
);
```

# configuration_archive

```sql
CREATE TABLE `configuration_archive`  (
  `id` bigint NOT NULL,
  `app_id` varchar(32) NOT NULL,
  `environment` varchar(50) NOT NULL,
  `data` text NULL,
  `operator` varchar(64) NOT NULL,
  `archive_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `configuration_archive_idx_unique`(`app_id` ASC, `environment` ASC) USING BTREE
);
```

# configuration_item

```sql
CREATE TABLE `configuration_item`  (
  `id` bigint NOT NULL,
  `configuration_id` bigint NOT NULL,
  `key` varchar(255) NOT NULL,
  `value` text NULL,
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `updated_by` varchar(64) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `configuration_item_idx_unique`(`configuration_id` ASC, `key` ASC) USING BTREE,
  INDEX `configuration_item_idx_fk`(`configuration_id` ASC) USING BTREE
);
```

# configuration_revision

```sql
CREATE TABLE `configuration_revision`  (
  `id` bigint NOT NULL,
  `configuration_id` bigint NOT NULL,
  `data` text NULL,
  `comment` varchar(1000) NULL DEFAULT NULL,
  `version` varchar(25) NOT NULL,
  `operator` varchar(64) NOT NULL,
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `configuration_revision_idx_fk`(`configuration_id` ASC) USING BTREE
);
```

# team

```sql
CREATE TABLE `team`  (
  `id` varchar(32) NOT NULL,
  `name` varchar(100) NOT NULL,
  `description` varchar(500) NULL DEFAULT NULL,
  `owner_id` varchar(32) NOT NULL,
  `member_count` int NOT NULL DEFAULT 0,
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(64) NOT NULL,
  `updated_by` varchar(64) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `team_idx_name`(`name` ASC) USING BTREE,
  INDEX `team_idx_owner_id`(`owner_id` ASC) USING BTREE
);
```

# team_member

```sql
CREATE TABLE `team_member`  (
  `id` bigint NOT NULL,
  `team_id` varchar(32) NOT NULL,
  `user_id` varchar(32) NOT NULL,
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `team_member_idx_unique`(`team_id` ASC, `user_id` ASC) USING BTREE
);
```

# token

```sql
CREATE TABLE `token`  (
  `id` bigint NOT NULL,
  `type` varchar(32) NOT NULL,
  `key` varchar(255) NOT NULL,
  `subject` varchar(20) NOT NULL,
  `issues` datetime NOT NULL,
  `expires` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `token_idx_key`(`key` ASC) USING BTREE,
  INDEX `token_idx_expires`(`expires` ASC) USING BTREE
);
```

# user

```sql
CREATE TABLE `user`  (
  `id` bigint NOT NULL,
  `username` varchar(64) NOT NULL,
  `password_hash` varchar(512) NOT NULL,
  `password_salt` varchar(32) NOT NULL,
  `nickname` varchar(100) NULL DEFAULT NULL,
  `email` varchar(255) NULL DEFAULT NULL,
  `phone` varchar(25) NULL DEFAULT NULL,
  `access_cailed_count` int NOT NULL DEFAULT 0,
  `lockout_end` datetime NULL DEFAULT NULL,
  `reserved` bit(1) NOT NULL DEFAULT b'0',
  `is_admin` bit(1) NOT NULL DEFAULT b'0',
  `source` int NOT NULL,
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `is_deleted` bit(1) NOT NULL DEFAULT b'0',
  `delete_time` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `user_idx_username`(`username` ASC) USING BTREE,
  UNIQUE INDEX `user_idx_email`(`email` ASC) USING BTREE,
  UNIQUE INDEX `user_idx_phone`(`phone` ASC) USING BTREE
);
```