using NATS.Client;

namespace Iot.WebAPI.Dispatcher
{
	public interface IEventDispatcher
	{	
		void Dispatch(object sender, MsgHandlerEventArgs e);
	}
}