using FabricCutter.UI.Logic.Events;
using FabricCutter.UI.Service;
using FabricCutter.UI.ViewModel;
using Microsoft.AspNetCore.Components;

namespace FabricCutter.UI.Logic
{
	public class SliderViewModel : ISliderViewModel
	{
		private readonly IEventHub _eventHub;
		private readonly IMarkerService _markerService;
				
		public int PointerPosition { get; set; }
		public int SliderLenght { get; set; }
		public List<Marker> Markers { get; set; }
		public Action StateHasChanged { get; set; } = () => { };

		public SliderViewModel(ApplicationSettings applicationSettings,
			IEventHub eventHub
		)
		{
			_eventHub = eventHub;
			SubscribeEvents();
			Markers = new();
			SliderLenght = applicationSettings.SliderLenght;
			PointerPosition = 0;
			SendPointerPositionChangeMesage();
		}


		

		public void OnPositionPointerChange(ChangeEventArgs e)
		{
			PointerPosition = Convert.ToInt32(e.Value);
			SendPointerPositionChangeMesage();
		}


		private void SendPointerPositionChangeMesage()
		{
			var args = new PointerPositionChangedEventArgs(PointerPosition);
			_eventHub.Publish(ApplicationEvents.OnPointerPositionChanged, args);
		}


		private void OnMarkerAddedhandler(ApplicationEvents applicationEvents, object value)
		{
			var message = EventArgsAdapter.GetEventArgs<MarkerAddedEventArgs>(applicationEvents, value);
			if (message is not null)
			{
				Markers = message.markersList;
				StateHasChanged();
			}
		}
		private void OnMarkerUpdatedHandler(ApplicationEvents applicationEvents, object value)
		{
			var message = EventArgsAdapter.GetEventArgs<MarkerUpdatedEventArgs>(applicationEvents, value);
			if (message is not null)
			{
				Markers = message.markersList;
				StateHasChanged();
			}
		}

		private void OnResetMarkerhandler(ApplicationEvents applicationEvents, object value)
		{
			var message = EventArgsAdapter.GetEventArgs<MarkerResetEventArgs>(applicationEvents, value);
			if (message is not null)
			{
				Markers.Clear();
				StateHasChanged();

			}
		}

		private void SubscribeEvents()
		{
			_eventHub.Subscribe(ApplicationEvents.OnMarkerAdded, OnMarkerAddedhandler);
			_eventHub.Subscribe(ApplicationEvents.OnMarkerUpdated, OnMarkerUpdatedHandler);
			_eventHub.Subscribe(ApplicationEvents.OnResetMarker, OnResetMarkerhandler);
		}

		private void UnsubscribeEvents()
		{
			_eventHub.Unsubscribe(ApplicationEvents.OnMarkerAdded, OnMarkerAddedhandler);
			_eventHub.Unsubscribe(ApplicationEvents.OnMarkerUpdated, OnMarkerUpdatedHandler);
			_eventHub.Unsubscribe(ApplicationEvents.OnResetMarker, OnResetMarkerhandler);
		}

		public void Dispose()
		{
			UnsubscribeEvents();
		}

		
	}
}
