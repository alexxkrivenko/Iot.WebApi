using System;
using System.Collections.Generic;
using Autofac;
using Iot.Events;
using Iot.WebAPI.EventHandlers;
using MessagePack;
using NATS.Client;
using NLog;

namespace Iot.WebAPI.Dispatcher
{
	public class EventDispatcher : IEventDispatcher
	{
		#region Delegates and events
		public void Dispatch(object sender, MsgHandlerEventArgs e)
		{
			var @event = (ArduinoInputEvent)MessagePackSerializer.Typeless.Deserialize(e.Message.Data);

			var handlers = GetEventHandlers(@event);
			if (handlers == null)
			{
				_logger.Warn("Для события: {0} не найдено обработчиков.", @event);
				return;
			}

			foreach (var handler in handlers)
			{
				handler.Handle(@event);
				_logger.Info($"Сообщение {@event.GetType().Name} обработано хэндлером {handler.GetType().Name}.");
			}
		}
		#endregion

		#region Data
		#region Fields
		private readonly IContainer _container;
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();
		#endregion
		#endregion

		#region .ctor
		public EventDispatcher(IContainer container)
		{
			_container = container ?? throw new ArgumentNullException(nameof(container));
		}
		#endregion

		#region Private
		private IList<IEventHandler> GetEventHandlers(IEvent @event)
		{
			var eventHandlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
			if (!_container.IsRegistered(eventHandlerType))
			{
				return null;
			}

			return new List<IEventHandler>((IList<IEventHandler>)
										   _container.Resolve(typeof(IEnumerable<>)
																  .MakeGenericType(eventHandlerType)));
		}
		#endregion
	}
}
