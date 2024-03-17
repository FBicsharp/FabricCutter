
namespace FabricCutter.UI.Logic.Events
{
	public interface IEventHub
	{
		void Publish(ApplicationEvents eventName, object payload);
		void Subscribe(ApplicationEvents eventName, Action<ApplicationEvents, object> callback);
		void Unsubscribe(ApplicationEvents eventName, Action<ApplicationEvents, object> callback);
	}
}