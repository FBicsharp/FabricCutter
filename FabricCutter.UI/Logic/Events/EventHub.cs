using Microsoft.AspNetCore.Components;

namespace FabricCutter.UI.Logic.Events
{

	public class EventHub : IEventHub
	{
		private readonly Dictionary<ApplicationEvents, Action<ApplicationEvents, object>> subscribers = new Dictionary<ApplicationEvents, Action<ApplicationEvents, object>>();

		public void Subscribe(ApplicationEvents eventName, Action<ApplicationEvents, object> callback)
		{
			if (!subscribers.ContainsKey(eventName))
			{
				subscribers[eventName] = callback;
			}
			else
			{
				subscribers[eventName] += callback;
			}
		}

		public void Unsubscribe(ApplicationEvents eventName, Action<ApplicationEvents, object> callback)
		{
			if (subscribers.ContainsKey(eventName))
			{
				subscribers[eventName] -= callback;
			}
		}

		public void Publish(ApplicationEvents eventName, object payload)
		{
			if (subscribers.ContainsKey(eventName))
			{				
				Console.WriteLine($"Event: {eventName} Payload: {payload}");                
                subscribers[eventName]?.Invoke(eventName, payload);
			}
		}
	}

	public enum ApplicationEvents
    {
        OnPointerPositionChanged,
        OnMarkerAdded,
        OnResetMarker,
        OnConfirmed,
		OnAddMarker,
		OnMarkerReseted
	}

    public record PointerPositionChangedEventArgs(int pointerPosition);
	public record MarkerAddEventArgs(Marker newMarker);
	public record MarkerAddedEventArgs(List<Marker> markersList);
	public record MarkerResetEventArgs();
	public record MarkerResetedEventArgs();

	public static class EventArgsAdapter
	{
		public static T GetEventArgs<T>(ApplicationEvents eventName, object args)
		{
			switch (eventName)
			{
				case ApplicationEvents.OnPointerPositionChanged:
					return args is PointerPositionChangedEventArgs pointerArgs ? (T)(object)pointerArgs : default;
				case ApplicationEvents.OnMarkerAdded:
					return args is MarkerAddedEventArgs markerArgs ? (T)(object)markerArgs : default;
				case ApplicationEvents.OnResetMarker:
					return args is MarkerResetedEventArgs resetArgs ? (T)(object)resetArgs : default;

				case ApplicationEvents.OnConfirmed:
					return (T)args;
				default:
					return default;
			}
		}
	}





}
