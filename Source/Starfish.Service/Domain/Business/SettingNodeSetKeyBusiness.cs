using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Domain;

// ReSharper disable MemberCanBePrivate.Global

namespace Nerosoft.Starfish.Domain;

public class SettingNodeSetKeyBusiness : EditableObject<SettingNodeSetKeyBusiness>, IDomainService
{
	private readonly ISettingNodeRepository _repository;

	public SettingNodeSetKeyBusiness(ISettingNodeRepository repository)
	{
		_repository = repository;
	}

	#region Properties

	public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(p => p.Id);
	public static readonly PropertyInfo<string> OldNameProperty = RegisterProperty<string>(p => p.OldName);
	public static readonly PropertyInfo<string> NewNameProperty = RegisterProperty<string>(p => p.NewName);

	public long Id
	{
		get => ReadProperty(IdProperty);
		private set => LoadProperty(IdProperty, value);
	}

	public string OldName
	{
		get => ReadProperty(OldNameProperty);
		private set => LoadProperty(OldNameProperty, value);
	}

	public string NewName
	{
		get => ReadProperty(NewNameProperty);
		private set => LoadProperty(NewNameProperty, value);
	}

	#endregion

	[FactoryCreate]
	protected void Create(long id, string oldName, string newName)
	{
		Id = id;
		OldName = oldName;
		NewName = newName;
	}

	[FactoryUpdate]
	protected override async Task UpdateAsync()
	{
		var aggregate = await _repository.GetAsync(Id, true, Array.Empty<string>());
		if (aggregate == null)
		{
			throw new SettingNodeNotFoundException(Id);
		}

		var parent = await _repository.GetAsync(aggregate.ParentId);

		if (parent == null)
		{
			throw new SettingNodeNotFoundException(aggregate.ParentId);
		}

		var entities = new List<SettingNode>();

		var newKey = parent.GenerateKey(NewName, aggregate.Sort);

		if (aggregate.Type is SettingNodeType.Array or SettingNodeType.Object)
		{
			var leaves = await _repository.GetLeavesAsync(aggregate.AppId, aggregate.Environment, aggregate.Key);

			foreach (var node in leaves)
			{
				node.SetKey(newKey + node.Key[aggregate.Key.Length..]);
			}

			entities.AddRange(leaves);
		}

		aggregate.SetKey(newKey);
		entities.Add(aggregate);

		await _repository.UpdateAsync(entities);
	}
}