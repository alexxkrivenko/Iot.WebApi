using System.Threading.Tasks;
using Iot.Events;

namespace Iot.WebAPI.EventHandlers
{
	public abstract class IEventHandler<TEvent> : IEventHandler where TEvent : IEvent
	{
		public abstract Task Handle(TEvent @event);

		public async Task Handle(IEvent @event)
		{
			await Handle((TEvent)@event);
		}
	}

	public interface IEventHandler
	{
		Task Handle(IEvent @event);
	}
}