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
		OnAddMarker,
		OnMarkerAdded,
		OnUpdateMarker,
		OnMarkerUpdated,
		OnResetMarker,
		OnMarkerReseted,
		OnExportRecipe,
		OnExportedRecipe,
		OnConfirmed
	}

	public record PointerPositionChangedEventArgs(int pointerPosition);
	public record MarkerAddEventArgs(Marker newMarker);
	public record MarkerAddedEventArgs(List<Marker> markersList);
	public record MarkerUpdateEventArgs(Marker updatedMarker);
	public record MarkerUpdatedEventArgs(List<Marker> markersList);
	public record MarkerResetEventArgs();
	public record MarkerResetedEventArgs();
	public record MarkerExportEventArgs();
	public record MarkerExportedEventArgs(List<Marker> markersList);

	public static class EventArgsAdapter
	{
		public static T GetEventArgs<T>(ApplicationEvents eventName, object args)
		{
			switch (eventName)
			{
				case ApplicationEvents.OnPointerPositionChanged:
					return args is PointerPositionChangedEventArgs pointerArgs ? (T)(object)pointerArgs : default;
				case ApplicationEvents.OnAddMarker:
					return args is MarkerAddEventArgs markerArgs ? (T)(object)markerArgs : default;
				case ApplicationEvents.OnMarkerAdded:
					return args is MarkerAddedEventArgs markerAddedArgs ? (T)(object)markerAddedArgs : default;
				case ApplicationEvents.OnUpdateMarker:
					return args is MarkerUpdateEventArgs updateArgs ? (T)(object)updateArgs : default;
				case ApplicationEvents.OnMarkerUpdated:
					return args is MarkerUpdatedEventArgs updatedArgs ? (T)(object)updatedArgs : default;
				case ApplicationEvents.OnResetMarker:
					return args is MarkerResetEventArgs resetArgs ? (T)(object)resetArgs : default;
				case ApplicationEvents.OnMarkerReseted:
					return args is MarkerResetedEventArgs resetedArgs ? (T)(object)resetedArgs : default;
				case ApplicationEvents.OnExportRecipe:
					return args is MarkerExportEventArgs exportArgs ? (T)(object)exportArgs : default;
				case ApplicationEvents.OnExportedRecipe:
					return args is MarkerExportedEventArgs exportedArgs ? (T)(object)exportedArgs : default;
				case ApplicationEvents.OnConfirmed:
					return (T)args;


				default:
					return default;
			}
		}
	}





}
