# operate_log

```sql
CREATE TABLE `operate_log`  (
  `Id` bigint NOT NULL,
  `Module` varchar(20) NOT NULL,
  `Type` varchar(50) NOT NULL,
  `Description` varchar(2000) NULL DEFAULT NULL,
  `UserName` varchar(64) NULL DEFAULT NULL,
  `OperateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Error` varchar(2000) NULL DEFAULT NULL,
  `RequestTraceId` varchar(40) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IDX_OPERATE_LOG_MODULE`(`Module` ASC) USING BTREE,
  INDEX `IDX_OPERATE_LOG_TYPE`(`Type` ASC) USING BTREE,
  INDEX `IDX_OPERATE_LOG_USER_NAME`(`UserName` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = Dynamic;
```

# configuration

```sql
CREATE TABLE `configuration`  (
  `Id` bigint NOT NULL,
  `TeamId` varchar(32) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Secret` varchar(255) NOT NULL,
  `Description` varchar(500) NULL DEFAULT NULL,
  `Status` int NOT NULL,
  `Version` varchar(20) NULL DEFAULT NULL,
  `PublishTime` datetime NULL DEFAULT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `CreatedBy` varchar(64) NOT NULL,
  `UpdatedBy` varchar(64) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IDX_CONFIG_UNIQUE`(`TeamId` ASC, `Name` ASC) USING BTREE,
  INDEX `IDX_CONFIG_TEAM_ID`(`TeamId` ASC) USING BTREE,
  INDEX `IDX_CONFIG_STATUS`(`Status` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = Dynamic;
```

# configuration_archive

```sql
CREATE TABLE `configuration_archive`  (
  `Id` bigint NOT NULL,
  `AppId` varchar(32) NOT NULL,
  `Environment` varchar(50) NOT NULL,
  `Data` text NULL,
  `Operator` varchar(64) NOT NULL,
  `ArchiveTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IDX_CONFIG_ARCHIVE_UNIQUE`(`AppId` ASC, `Environment` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = Dynamic;
```

# configuration_item

```sql
CREATE TABLE `configuration_item`  (
  `Id` bigint NOT NULL,
  `ConfigurationId` bigint NOT NULL,
  `Key` varchar(255) NOT NULL,
  `Value` text NULL,
  `UpdateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `UpdatedBy` varchar(64) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IDX_CONFIG_ITEM_UNIQUE`(`ConfigurationId` ASC, `Key` ASC) USING BTREE,
  INDEX `IDX_CONFIG_ITEM_FK`(`ConfigurationId` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = Dynamic;
```

# configuration_revision

```sql
CREATE TABLE `configuration_revision`  (
  `Id` bigint NOT NULL,
  `ConfigurationId` bigint NOT NULL,
  `Data` text NULL,
  `Comment` varchar(1000) NULL DEFAULT NULL,
  `Version` varchar(25) NOT NULL,
  `Operator` varchar(64) NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IDS_CONFIG_REVISION_FK`(`ConfigurationId` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = Dynamic;
```

# team

```sql
CREATE TABLE `team`  (
  `Id` varchar(32) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Description` varchar(500) NULL DEFAULT NULL,
  `OwnerId` varchar(32) NOT NULL,
  `MemberCount` int NOT NULL DEFAULT 0,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `CreatedBy` varchar(64) NOT NULL,
  `UpdatedBy` varchar(64) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IDX_TEAM_NAME`(`Name` ASC) USING BTREE,
  INDEX `IDX_TEAM_OWNER`(`OwnerId` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = Dynamic;
```

# team_member

```sql
CREATE TABLE `team_member`  (
  `Id` bigint NOT NULL,
  `TeamId` varchar(32) NOT NULL,
  `UserId` varchar(32) NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IDX_TEAM_MEMBER_UNIQUE`(`TeamId` ASC, `UserId` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = Dynamic;
```

# token

```sql
CREATE TABLE `token`  (
  `Id` bigint NOT NULL,
  `Type` varchar(32) NOT NULL,
  `Key` varchar(255) NOT NULL,
  `Subject` varchar(20) NOT NULL,
  `Issues` datetime NOT NULL,
  `Expires` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IDX_TOKEN_KEY`(`Key` ASC) USING BTREE,
  INDEX `IDX_TOKEN_EXPIRES`(`Expires` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = Dynamic;
```

# user

```sql
CREATE TABLE `user`  (
  `Id` varchar(32) NOT NULL,
  `UserName` varchar(64) NOT NULL,
  `PasswordHash` varchar(512) NOT NULL,
  `PasswordSalt` varchar(32) NOT NULL,
  `NickName` varchar(100) NULL DEFAULT NULL,
  `Email` varchar(255) NULL DEFAULT NULL,
  `Phone` varchar(25) NULL DEFAULT NULL,
  `AccessFailedCount` int NOT NULL DEFAULT 0,
  `LockoutEnd` datetime NULL DEFAULT NULL,
  `Reserved` bit(1) NOT NULL DEFAULT b'0',
  `IsAdmin` bit(1) NOT NULL DEFAULT b'0',
  `Source` int NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `IsDeleted` bit(1) NOT NULL DEFAULT b'0',
  `DeleteTime` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IDX_USER_USERNAME`(`UserName` ASC) USING BTREE,
  UNIQUE INDEX `IDX_USER_EMAIL`(`Email` ASC) USING BTREE,
  UNIQUE INDEX `IDX_USER_PHONE`(`Phone` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = Dynamic;
```