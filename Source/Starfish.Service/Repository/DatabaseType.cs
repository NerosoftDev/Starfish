namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 数据库类型枚举
/// </summary>
public enum DatabaseType
{
	/// <summary>
	/// Microsoft SQL Server
	/// </summary>
	SqlServer,

	/// <summary>
	/// MySQL
	/// </summary>
	MySql,

	/// <summary>
	/// PostgreSQL
	/// </summary>
	PostgreSql,

	/// <summary>
	/// SQLite
	/// </summary>
	Sqlite,

	/// <summary>
	/// MongoDB
	/// </summary>
	MongoDb,

	/// <summary>
	/// InMemory
	/// </summary>
	InMemory,
}