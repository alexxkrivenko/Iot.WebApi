using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Iot.Events;
using Iot.WebAPI;
using Iot.WebAPI.Dal;
using Iot.WebAPI.Dispatcher;
using Iot.WebAPI.EventHandlers;
using Iot.WebAPI.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using NLog;

namespace Iot.WebApi
{
	public class Startup
	{
		#region Data
		#region Static
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		#endregion
		#endregion

		#region .ctor
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		#endregion

		#region Properties
		public IContainer ApplicationContainer
		{
			get;
			private set;
		}

		public IConfiguration Configuration
		{
			get;
		}
		#endregion

		#region Public
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			//app.UseForwardedHeaders(new ForwardedHeadersOptions
			//{
			//	ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			//});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}
			app.UseSignalR(route =>
				{
					route.MapHub<EventsHub>("/events");
				});
			app.UseMvc();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddSignalR(opt =>
				{
					opt.HandshakeTimeout = TimeSpan.FromSeconds(30);
					opt.EnableDetailedErrors = true;
				});
			services.AddMvc()
					.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
					//позволяет резолвить зависимости в контроллеры.
					.AddControllersAsServices();

			services.AddTransient<EventsHub>();

			var configuration = new AppConfiguration(Configuration);
			services.AddSingleton(configuration);

			services.AddAutoMapper(Assembly.GetExecutingAssembly());

			var builder = new ContainerBuilder();
			RegisterDbServices(services);

			builder.Populate(services);

			RegisterEventHandlers(builder);

			ApplicationContainer = builder.Build();

			RegisterNats(configuration, services, ApplicationContainer);

			//все контроллеры резолвятся через AutoFac
			return new AutofacServiceProvider(ApplicationContainer);
		}
		#endregion

		#region Private
		private Type[] GetEventTypes()
		{
			return Assembly.GetAssembly(typeof(IEvent))
						   .GetExportedTypes()
						   .Where(t => typeof(IEvent).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
						   .ToArray();
		}

		private void RegisterDbServices(IServiceCollection services)
		{
			services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase(databaseName: "iotbase"));
		}

		private void RegisterEventHandlers(ContainerBuilder builder)
		{
			builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IEventHandler)))
				   .AsClosedTypesOf(typeof(IEventHandler<>))
				   .AsImplementedInterfaces();
		}

		private void RegisterNats(AppConfiguration configuration, IServiceCollection services, IContainer container)
		{
			IConnection natsConnection = null;
			try
			{
				natsConnection = new ConnectionFactory()
					.CreateConnection($"{configuration.ServerName}:{configuration.ServerPort}");
			}
			catch(Exception)
			{
				Logger.Fatal("Подключение к серверу NATS не установлено.");
				throw;
			}

			services.AddSingleton(natsConnection);
			SubscribeEvents(natsConnection, new EventDispatcher(container));
		}

		private void SubscribeEvents(IConnection connection, IEventDispatcher dispatcher)
		{
			foreach (var @event in GetEventTypes())
			{
				connection.SubscribeAsync(@event.Name, dispatcher.Dispatch);
				Logger.Info("Выполнена подписка на событие: {0}.", @event.Name);
			}
		}
		#endregion
	}
}
