using System;
using System.Threading.Tasks;
using Iot.Events;
using Iot.WebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Iot.WebAPI.EventHandlers
{
	public class ArduinoInputEventHandler : IEventHandler<ArduinoInputEvent>
	{
		#region Data
		#region Fields
		private readonly IHubContext<EventsHub> _hub;
		#endregion
		#endregion

		#region .ctor
		public ArduinoInputEventHandler(IHubContext<EventsHub> hub)
		{
			_hub = hub ?? throw new ArgumentNullException(nameof(hub));
		}
		#endregion

		#region Overrided
		public override async Task Handle(ArduinoInputEvent @event)
		{
			await _hub.Clients.All.SendAsync("ReceiveEvent", @event);
		}
		#endregion
	}
}
