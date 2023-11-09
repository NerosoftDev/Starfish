# Overview/概览 ⚡

[![Build Status](https://travis-ci.com/realzhaorong/Starfish.svg?branch=master)]()
[![License](https://img.shields.io/badge/license-AGPL--3.0-blue.svg)](LICENSE)
[![GitHub release](https://img.shields.io/github/release/realzhaorong/Starfish.svg)]()
[![GitHub stars](https://img.shields.io/github/stars/realzhaorong/Starfish.svg)]()

Starfish is a lightweight powerful distributed configuration server for .NET application.

Starfish是一个轻量但功能强大的分布式 .NET 应用程序配置中心。

## ⌨️ Features/功能

> 💚 Completed/已完成 ⌛ In progress/进行中 🕝 Planned/计划中

- [ ] 🕝 Support multiple data sources/支持多种数据源
    - [ ] 🕝 MySQL
    - [ ] 🕝 SqlServer
    - [ ] 🕝 PostgreSQL
    - [ ] 🕝 MongoDB
- [ ] 🕝 Support multiple node deployment/支持多节点部署
- [ ] 🕝 Support multiple environments/支持多环境
- [ ] 🕝 Deploy with docker/支持Docker部署
- [ ] 🕝 Support client cache/支持客户端缓存
- [ ] 🕝 Multiple protocols support/支持多种协议
    - [ ] 🕝 HTTP
    - [ ] 🕝 gRPC
    - [ ] 🕝 WebSocket
- [ ] 🕝 Rollback to history version/回滚到历史版本
- [ ] 🕝 Role-based access control/基于角色的访问控制
- [ ] 🕝 Support multiple languages admin panel/支持多语言管理面板
    - [ ] 🕝 en/英语
    - [ ] 🕝 zh-Hans/简体中文
    - [ ] 🕝 zh-Hant/繁体中文
    
## 💰 Donate/捐助 
<img alt="" title="donate" width="512" src="https://qiniu-cdn.zhaorong.pro/images/donate.png" /> 

**Paypal**

[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.me/realzhaorong)

[https://www.paypal.me/realzhaorong](https://www.paypal.me/realzhaorong)

---

> If you like my work, you can support me by donation. / 如果您喜欢我的工作，可以通过捐赠来支持我。

## 📨 Contact and Suggestions/联系与建议

Any feedback is welcome, you can create a issue, or contact us by email, thank you.

非常乐意收到您的任何反馈，您可以创建一个 issue，或通过邮件联系我们，谢谢。

> [zhaorong@outlook.com](mailto:zhaorong@outlook.com)

## 🔑 License/许可证

This project is licensed under the AGPL-3.0 License - see the [LICENSE](LICENSE) file for details.

本项目采用 AGPL-3.0 协议，可查看 [LICENSE](LICENSE) 了解更详细内容。


# Getting Started/快速开始 ⚡

## Solution structure/解决方案结构

## Requirements/环境要求

### Development/开发环境

IDE/开发环境
- Windows/Linux/MacOS
- Visual Studio/Visual Studio Code/Rider. Visual Studio for Mac is retired, so it's not recommended./Visual Studio for Mac 已经被微软弃用，所以不推荐使用。
- .NET 8 SDK.

Dependencies/依赖服务
- Redis 6 and above. It's required for distributed cache and distributed lock. / 如果您需要使用分布式缓存和分布式锁，那么Redis是必须的。
- MySQL/SQL Server/PostgreSQL/MongoDB, latest version recommended. The database service is used to store configuration data. Choose one that you are about to use for production. / 数据库服务用于存储配置数据。请选择您将要在生产环境中使用的数据库。
- RabbitMQ

Deploy & Run/部署与运行
- Docker & Docker Compose

### Production/生产环境

Runtime/运行环境
- Windows/Linux, CentOS/Ubuntu recommended.
- .NET 8 Runtime. It's not needed if running in container./如果您在容器中运行，那么您不需要安装.NET 8 Runtime。

Dependencies/依赖服务
- Redis 6 and above. It's required for distributed cache and distributed lock. / 如果您需要使用分布式缓存和分布式锁，那么Redis是必须的。
- MySQL/SQL Server/PostgreSQL/MongoDB, latest version recommended. The database service is used to store configuration data. Choose one that you are about to use for production. / 数据库服务用于存储配置数据。请选择您将要在生产环境中使用的数据库。
- RabbitMQ

Deploy & Run/部署与运行
- Docker & Docker Compose. As you can see, we recommend you to run Starfish in container. / 如您所见，我们建议您在容器中运行Starfish。

## Server/服务端

### Deploy/部署

```bash
```

### Configuration/配置

```bash
```

## Client/客户端

### Install/安装

```bash
```

### Configuration/配置

```bash
```


# Documentation/文档 ⚡

## API

## Resources/资源

## Release Notes/发布日志

## FAQ/常见问题


# Roadmap/路线图 ⚡


# Contributing/贡献 ⚡


# Acknowledgements/鸣谢 ⚡

[![JetBrains](https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.svg)](https://www.jetbrains.com/)

Thanks to JetBrains for supporting the project through All Products Packs within their Free Open Source License program.

感谢JetBrains通过其免费开源许可计划中的所有产品包支持该项目。