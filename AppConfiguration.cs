using System;
using Microsoft.Extensions.Configuration;

namespace Iot.WebAPI
{
	public class AppConfiguration
	{
		#region Data
		#region Fields
		private readonly IConfiguration _configuration;
		#endregion
		#endregion

		#region .ctor
		public AppConfiguration(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public string ServerName
		{
			get => _configuration["NatsSettings:Server"];
		}

		public string ServerPort
		{
			get => _configuration["NatsSettings:Port"];
		}
		#endregion
	}
}
