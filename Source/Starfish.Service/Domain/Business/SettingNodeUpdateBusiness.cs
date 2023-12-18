using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;

// ReSharper disable MemberCanBePrivate.Global

namespace Nerosoft.Starfish.Domain;

/// <summary>
/// 配置节点重命名领域服务
/// </summary>
public class SettingNodeUpdateBusiness : EditableObject<SettingNodeUpdateBusiness>, IDomainService
{
	private readonly ISettingNodeRepository _repository;

	public SettingNodeUpdateBusiness(ISettingNodeRepository repository)
	{
		_repository = repository;
	}

	#region Register properties

	public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(p => p.Id);

	public static readonly PropertyInfo<string> ValueProperty = RegisterProperty<string>(p => p.Value);

	public static readonly PropertyInfo<string> IntentProperty = RegisterProperty<string>(p => p.Intent);

	#endregion

	#region Defines properties

	public long Id
	{
		get => ReadProperty(IdProperty);
		private set => LoadProperty(IdProperty, value);
	}

	public string Value
	{
		get => ReadProperty(ValueProperty);
		internal set => LoadProperty(ValueProperty, value);
	}

	public string Intent
	{
		get => ReadProperty(IntentProperty);
		internal set => LoadProperty(IntentProperty, value);
	}

	public SettingNode Aggregate { get; private set; }

	#endregion

	[FactoryFetch]
	protected async Task FetchAsync(long id)
	{
		var aggregate = await _repository.GetAsync(Id, true, []);

		Aggregate = aggregate ?? throw new SettingNodeNotFoundException(Id);

		Id = id;
	}

	[FactoryUpdate]
	protected override async Task UpdateAsync()
	{
		Func<SettingNode, Task> action = Intent switch
		{
			"name" => UpdateNameAsync,
			"desc" => UpdateDescriptionAsync,
			"description" => UpdateDescriptionAsync,
			"value" => UpdateValueAsync,
			_ => _ => Task.CompletedTask
		};

		await action(Aggregate);

		await _repository.UpdateAsync(Aggregate);
	}

	private async Task UpdateNameAsync(SettingNode aggregate)
	{
		if (aggregate.Type == SettingNodeType.Root)
		{
			throw new ForbiddenException(Resources.IDS_ERROR_NOT_ALLOW_RENAME_ROOT_NODE);
		}

		var parent = await _repository.GetAsync(aggregate.ParentId);

		if (parent is { Type: SettingNodeType.Array })
		{
			throw new ForbiddenException(Resources.IDS_ERROR_NOT_ALLOW_RENAME_ARRAY_CHILD_NODE);
		}

		var exists = await _repository.CheckChildNodeNameAsync(aggregate.Id, aggregate.ParentId, Value);
		if (exists)
		{
			throw new ForbiddenException(Resources.IDS_ERROR_SETTING_NODE_NAME_EXISTS);
		}

		aggregate.SetName(Value);
	}

	private Task UpdateValueAsync(SettingNode aggregate)
	{
		aggregate.SetValue(Value);
		return Task.CompletedTask;
	}

	private Task UpdateDescriptionAsync(SettingNode aggregate)
	{
		aggregate.SetDescription(Value);
		return Task.CompletedTask;
	}
}