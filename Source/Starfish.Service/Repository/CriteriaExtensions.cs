using Nerosoft.Euonia.Linq;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// 查询条件扩展方法
/// </summary>
public static class CriteriaExtensions
{
	/// <summary>
	/// 获取日志查询规约
	/// </summary>
	/// <param name="criteria"></param>
	/// <returns></returns>
	public static Specification<OperateLog> GetSpecification(this OperateLogCriteria criteria)
	{
		Specification<OperateLog> specification = new TrueSpecification<OperateLog>();

		if (criteria == null)
		{
			return specification;
		}

		if (!string.IsNullOrWhiteSpace(criteria.UserName))
		{
			specification &= (Specification<OperateLog>)OperateLogSpecification.UserNameContains(criteria.UserName);
		}

		if (!string.IsNullOrWhiteSpace(criteria.Type))
		{
			specification &= (Specification<OperateLog>)OperateLogSpecification.TypeEquals(criteria.Type);
		}

		if (criteria.MinTime > DateTime.MinValue)
		{
			specification &= (Specification<OperateLog>)OperateLogSpecification.TimeAfter(criteria.MinTime.Value);
		}

		if (criteria.MaxTime > DateTime.MinValue)
		{
			specification &= (Specification<OperateLog>)OperateLogSpecification.TimeBefore(criteria.MaxTime.Value);
		}

		return specification;
	}
}