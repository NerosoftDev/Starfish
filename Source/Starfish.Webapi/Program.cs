using Nerosoft.Euonia.Hosting;
using Nerosoft.Starfish.Webapi;

namespace Starfish.Webapi;

/// <summary>
/// Program
/// </summary>
public class Program
{
	/// <summary>
	/// 入口函数
	/// </summary>
	/// <param name="args"></param>
	public static void Main(string[] args)
	{
		HostUtility.Run<Startup>(args, HostBuilderOptionsAction);
		
		static void HostBuilderOptionsAction(HostBuilderOptions options)
		{
			options.EnableHttp2 = true;
			options.UseSerilog = true;
		}
	}
}
