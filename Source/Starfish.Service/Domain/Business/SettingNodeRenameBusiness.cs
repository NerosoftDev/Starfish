using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;

// ReSharper disable MemberCanBePrivate.Global

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置节点重命名领域服务
/// </summary>
public class SettingNodeRenameBusiness : EditableObject<SettingNodeRenameBusiness>, IDomainService
{
	private readonly ISettingNodeRepository _repository;

	public SettingNodeRenameBusiness(ISettingNodeRepository repository)
	{
		_repository = repository;
	}

	#region Register properties

	public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(p => p.Id);

	public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);

	#endregion

	#region Defines properties

	public long Id
	{
		get => ReadProperty(IdProperty);
		private set => LoadProperty(IdProperty, value);
	}

	public string Name
	{
		get => ReadProperty(NameProperty);
		private set => LoadProperty(NameProperty, value);
	}

	#endregion

	[FactoryCreate]
	protected void Create(long id, string name)
	{
		Id = id;
		Name = name;
	}

	[FactoryUpdate]
	protected override async Task UpdateAsync()
	{
		var aggregate = await _repository.GetAsync(Id, true, []);

		if (aggregate == null)
		{
			throw new SettingNodeNotFoundException(Id);
		}

		if (aggregate.Type == SettingNodeType.Root)
		{
			throw new ForbiddenException(Resources.IDS_ERROR_NOT_ALLOW_RENAME_ROOT_NODE);
		}

		var parent = await _repository.GetAsync(aggregate.ParentId);

		if (parent is { Type: SettingNodeType.Array })
		{
			throw new ForbiddenException(Resources.IDS_ERROR_NOT_ALLOW_RENAME_ARRAY_CHILD_NODE);
		}

		var exists = await _repository.CheckChildNodeNameAsync(aggregate.Id, aggregate.ParentId, Name);
		if (exists)
		{
			throw new ForbiddenException(Resources.IDS_ERROR_SETTING_NODE_NAME_EXISTS);
		}

		aggregate.SetName(Name);
		await _repository.UpdateAsync(aggregate);
	}
}