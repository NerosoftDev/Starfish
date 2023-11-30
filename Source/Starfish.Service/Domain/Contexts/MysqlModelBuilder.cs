using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nerosoft.Starfish.Service;

namespace Nerosoft.Starfish.Domain;

internal class MysqlModelBuilder : IModelBuilder
{
	public void Configure(ModelBuilder modelBuilder)
	{
		throw new NotImplementedException();
	}
}
