using Nerosoft.Euonia.Bus;
using Nerosoft.Euonia.Business;
using Nerosoft.Euonia.Linq;
using Nerosoft.Euonia.Repository;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Repository;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// 应用配置命令处理器
/// </summary>
public class SettingNodeCommandHandler : CommandHandlerBase,
                                         IHandler<SettingRootNodeCreateCommand>,
                                         IHandler<SettingLeafNodeCreateCommand>,
                                         IHandler<SettingNodeUpdateCommand>,
                                         IHandler<SettingNodeDeleteCommand>,
                                         IHandler<SettingNodePublishCommand>,
                                         IHandler<SettingNodeSetKeyCommand>
{
	private readonly IServiceProvider _provider;

	private ISettingNodeRepository _settingRepository;
	private ISettingNodeRepository SettingRepository => _settingRepository ??= _provider.GetService<ISettingNodeRepository>();

	private IAppInfoRepository _appInfoRepository;
	private IAppInfoRepository AppInfoRepository => _appInfoRepository ??= _provider.GetService<IAppInfoRepository>();

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
				throw new ConflictException(Resources.IDS_ERROR_SETTING_NODE_DUPLICATE_ROOT);
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
					throw new InvalidOperationException(Resources.IDS_ERROR_NOT_ALLOW_ADD_ROOT_IN_CHILD_NODE);
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
			var business = await Factory.FetchAsync<SettingNodeUpdateBusiness>(message.Id);
			business.Intent = message.Intent;
			business.Value = message.Value;
			business.MarkAsUpdate();
			await business.SaveAsync(true, cancellationToken);
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

			var specifications = new List<ISpecification<SettingNode>>();

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
					specifications.Add(SettingNodeSpecification.AppIdEquals(aggregate.AppId));
					specifications.Add(SettingNodeSpecification.EnvironmentEquals(aggregate.Environment));
					specifications.Add(SettingNodeSpecification.TypeIn(types));
				}
					break;
				case SettingNodeType.Object:
				case SettingNodeType.Array:
				{
					specifications.Add(SettingNodeSpecification.AppIdEquals(aggregate.AppId));
					specifications.Add(SettingNodeSpecification.EnvironmentEquals(aggregate.Environment));
					specifications.Add(SettingNodeSpecification.KeyStartsWith(aggregate.Key));
				}
					break;
			}

			if (specifications.Count > 0)
			{
				var predicate = new CompositeSpecification<SettingNode>(PredicateOperator.AndAlso, specifications.ToArray()).Satisfy();

				var leaves = await SettingRepository.FindAsync(predicate, true, [], cancellationToken);
				nodes.AddRange(leaves);
			}

			await SettingRepository.DeleteAsync(nodes, true, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(SettingNodePublishCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			await Factory.ExecuteAsync<SettingPublishBusiness>(message.Id, cancellationToken);
		});
	}

	/// <inheritdoc />
	public Task HandleAsync(SettingNodeSetKeyCommand message, MessageContext context, CancellationToken cancellationToken = default)
	{
		return ExecuteAsync(async () =>
		{
			var business = await Factory.CreateAsync<SettingNodeSetKeyBusiness>(message.Item1, message.Item2, message.Item3);
			business.MarkAsUpdate();
			await business.SaveAsync(true, cancellationToken);
		});
	}
}