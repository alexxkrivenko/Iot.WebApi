using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Iot.WebApi
{
	public class Program
	{
		#region Public
		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				   .UseStartup<Startup>()
				   .ConfigureLogging(logging =>
					   {
						   logging.ClearProviders();
						   logging.SetMinimumLevel(LogLevel.Trace);
					   })
				   .UseNLog();

		public static void Main(string[] args)
		{
			var host = CreateWebHostBuilder(args)
				.Build();
			host.Run();
		}
		#endregion
	}
}
