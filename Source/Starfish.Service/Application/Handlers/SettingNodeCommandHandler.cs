using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用配置命令处理器
/// </summary>
public class SettingNodeCommandHandler : CommandHandlerBase,
                                         IHandler<SettingRootNodeCreateCommand>,
                                         IHandler<SettingLeafNodeCreateCommand>,
                                         IHandler<SettingNodeUpdateCommand>,
                                         IHandler<SettingNodeRenameCommand>,
                                         IHandler<SettingNodeDeleteCommand>
{
	private readonly IServiceProvider _provider;

	private ISettingNodeRepository _settingRepository;
	private ISettingNodeRepository SettingRepository => _settingRepository ??= _provider.GetService<ISettingNodeRepository>();

	private IAppInfoRepository _appInfoRepository;
	private IAppInfoRepository AppInfoRepository => _appInfoRepository ??= _provider.GetService<IAppInfoRepository>();

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="unitOfWork"></param>
	/// <param name="factory"></param>
	/// <param name="provider"></param>
	public SettingNodeCommandHandler(IUnitOfWorkManager unitOfWork, IObjectFactory factory, IServiceProvider provider)
		: base(unitOfWork, factory)
	{
		_provider = provider;
	}

	/// <inheritdoc />
	public Task HandleAsync(SettingRootNodeCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var appinfo = await AppInfoRepository.GetAsync(message.AppId, cancellationToken);
			if (appinfo == null)
			{
				throw new AppInfoNotFoundException(message.AppId);
			}

			var exists = await SettingRepository.ExistsAsync(message.AppId, message.Environment, cancellationToken);
			if (exists)
			{
				throw new ConflictException("根节点已存在");
			}

			var entity = SettingNode.CreateRoot(appinfo.Id, appinfo.Code, message.Environment);
			await SettingRepository.InsertAsync(entity, true, cancellationToken);
			return entity.Id;
		}, context.Response);
	}

	/// <inheritdoc />
	public Task HandleAsync(SettingLeafNodeCreateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var aggregate = await SettingRepository.GetAsync(message.Id, cancellationToken);
			if (aggregate == null)
			{
				throw new SettingNodeNotFoundException(message.Id);
			}

			SettingNode entity;
			switch (message.Type)
			{
				case SettingNodeType.Array:
					aggregate.AddArrayNode(message.Data.Name, out entity);
					break;
				case SettingNodeType.Object:
					aggregate.AddObjectNode(message.Data.Name, out entity);
					break;
				case SettingNodeType.String:
					aggregate.AddStringNode(message.Data.Name, message.Data.Value, out entity);
					break;
				case SettingNodeType.Number:
					aggregate.AddNumberNode(message.Data.Name, message.Data.Value, out entity);
					break;
				case SettingNodeType.Boolean:
					aggregate.AddBooleanNode(message.Data.Name, message.Data.Value, out entity);
					break;
				case SettingNodeType.Root:
					throw new InvalidOperationException("不能在子节点增加根节点");
				case SettingNodeType.Referer:
				default:
					throw new ArgumentOutOfRangeException(nameof(message.Type), message.Type, null);
			}

			await SettingRepository.UpdateAsync(aggregate, true, cancellationToken);
			return entity?.Id ?? 0;
		}, context.Response);
	}

	/// <inheritdoc />
	public Task HandleAsync(SettingNodeUpdateCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var properties = new[]
			{
				nameof(SettingNode.Parent),
				nameof(SettingNode.Children)
			};
			var aggregate = await SettingRepository.GetAsync(message.Item1, true, properties, cancellationToken);
			if (aggregate == null)
			{
				throw new SettingNodeNotFoundException(message.Item1);
			}
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(SettingNodeRenameCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var aggregate = await SettingRepository.GetAsync(message.Item1, cancellationToken);
			if (aggregate == null)
			{
				throw new SettingNodeNotFoundException(message.Item1);
			}

			if (aggregate.Type == SettingNodeType.Root)
			{
				throw new InvalidOperationException("不能设置根节点名称");
			}

			if (string.IsNullOrWhiteSpace(message.Item2))
			{
				throw new BadRequestException("节点名称不能为空");
			}

			if (string.Equals(message.Item2, aggregate.Name))
			{
				return;
			}

			var parent = await SettingRepository.GetAsync(aggregate.ParentId, cancellationToken);
			if (parent == null)
			{
				throw new InternalServerErrorException("父节点不存在");
			}

			if (parent.Type == SettingNodeType.Array)
			{
				throw new InvalidOperationException("数组节点的子节点不能修改名称");
			}

			if (parent.Children.Any(x => x.Id != aggregate.Id && x.Name == message.Item2))
			{
				throw new ConflictException("同级节点名称已存在");
			}

			aggregate.Name = message.Item2;

			var newKey = parent.GenerateKey(message.Item2, aggregate.Sort);

			var leaves = await SettingRepository.GetLeavesAsync(aggregate.AppId, aggregate.Environment, aggregate.Key, cancellationToken);
			foreach (var node in leaves)
			{
				node.Key = newKey + ":" + node.Key[aggregate.Key.Length..];
			}

			aggregate.Key = newKey;
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(SettingNodeDeleteCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var aggregate = await SettingRepository.GetAsync(message.Item1, cancellationToken);
			if (aggregate == null)
			{
				throw new SettingNodeNotFoundException(message.Item1);
			}

			List<SettingNode> nodes = [aggregate];

			switch (aggregate.Type)
			{
				case SettingNodeType.Root:
				{
					var types = new[]
					{
						SettingNodeType.Array,
						SettingNodeType.Object,
						SettingNodeType.String,
						SettingNodeType.Boolean,
						SettingNodeType.Number,
						SettingNodeType.Referer
					};
					var leaves = await SettingRepository.GetNodesAsync(aggregate.AppId, aggregate.Environment, types, cancellationToken);
					nodes.AddRange(leaves);
				}
					break;
				case SettingNodeType.Object:
				case SettingNodeType.Array:
				{
					var leaves = await SettingRepository.GetLeavesAsync(aggregate.AppId, aggregate.Environment, aggregate.Key, cancellationToken);
					nodes.AddRange(leaves);
				}
					break;
			}

			await SettingRepository.DeleteAsync(nodes, true, cancellationToken);
		});
	}
}