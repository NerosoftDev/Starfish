using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 设置节点规约
/// </summary>
public static class SettingNodeSpecification
{
	/// <summary>
	/// Id等于<paramref name="id"/>
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static Specification<SettingNode> IdEquals(long id)
	{
		return new DirectSpecification<SettingNode>(x => x.Id == id);
	}

	/// <summary>
	/// Id在<paramref name="ids"/>中
	/// </summary>
	/// <param name="ids"></param>
	/// <returns></returns>
	public static Specification<SettingNode> IdIn(IEnumerable<long> ids)
	{
		return new DirectSpecification<SettingNode>(x => ids.Contains(x.Id));
	}

	/// <summary>
	/// Id不等于<paramref name="id"/>
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static Specification<SettingNode> IdNotEquals(long id)
	{
		return new DirectSpecification<SettingNode>(x => x.Id != id);
	}

	/// <summary>
	/// 父节点Id等于<paramref name="parentId"/>
	/// </summary>
	/// <param name="parentId">父节点Id</param>
	/// <returns></returns>
	public static Specification<SettingNode> ParentIdEquals(long parentId)
	{
		return new DirectSpecification<SettingNode>(x => x.ParentId == parentId);
	}

	/// <summary>
	/// AppId等于<paramref name="appId"/>
	/// </summary>
	/// <param name="appId"></param>
	/// <returns></returns>
	public static Specification<SettingNode> AppIdEquals(long appId)
	{
		return new DirectSpecification<SettingNode>(x => x.AppId == appId);
	}

	/// <summary>
	/// AppCode等于<paramref name="appCode"/>
	/// </summary>
	/// <param name="appCode"></param>
	/// <returns></returns>
	public static Specification<SettingNode> AppCodeEquals(string appCode)
	{
		return new DirectSpecification<SettingNode>(x => x.AppCode == appCode);
	}

	/// <summary>
	/// 环境等于<paramref name="environment"/>
	/// </summary>
	/// <param name="environment"></param>
	/// <returns></returns>
	public static Specification<SettingNode> EnvironmentEquals(string environment)
	{
		return new DirectSpecification<SettingNode>(x => x.Environment == environment);
	}

	/// <summary>
	/// 类型等于<paramref name="type"/>
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public static Specification<SettingNode> TypeEquals(SettingNodeType type)
	{
		return new DirectSpecification<SettingNode>(x => x.Type == type);
	}

	/// <summary>
	/// 类型在<paramref name="types"/>中
	/// </summary>
	/// <param name="types"></param>
	/// <returns></returns>
	public static Specification<SettingNode> TypeIn(IEnumerable<SettingNodeType> types)
	{
		return new DirectSpecification<SettingNode>(x => types.Contains(x.Type));
	}

	/// <summary>
	/// Key等于<paramref name="key"/>
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public static Specification<SettingNode> KeyEquals(string key)
	{
		return new DirectSpecification<SettingNode>(x => x.Key == key);
	}

	/// <summary>
	/// Key以<paramref name="key"/>开头
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public static Specification<SettingNode> KeyStartsWith(string key)
	{
		return new DirectSpecification<SettingNode>(x => x.Key.StartsWith(key));
	}

	/// <summary>
	/// 名称等于<paramref name="name"/>
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public static Specification<SettingNode> NameEquals(string name)
	{
		name = name.Normalize(TextCaseType.Lower);
		return new DirectSpecification<SettingNode>(x => x.Name.ToLower() == name);
	}

	/// <summary>
	/// 根节点Id等于<paramref name="rootId"/>.
	/// </summary>
	/// <param name="rootId"></param>
	/// <returns></returns>
	public static Specification<SettingNode> RootIdEquals(long rootId)
	{
		return new DirectSpecification<SettingNode>(x => x.RootId == rootId);
	}
}