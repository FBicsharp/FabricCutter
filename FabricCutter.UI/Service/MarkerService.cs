﻿
using FabricCutter.UI.Logic;
using FabricCutter.UI.Logic.Events;


namespace FabricCutter.UI.Service
{
	public class MarkerService : IMarkerService
	{
		private readonly ILogger<MarkerService> _logger;
		private readonly IEventHub _eventHub;

		private List<Marker> InMemoryMarker { get; set; }


		public MarkerService(ILogger<MarkerService> logger, IEventHub eventHub)
		{
			_logger = logger;
			InMemoryMarker = new();
			_eventHub = eventHub;
			InitializeSubscribeEvents();
		}


		private void InitializeSubscribeEvents()
		{
			_eventHub.Subscribe(ApplicationEvents.OnAddMarker,
				async (applicationEvents, value) =>
				{
					var message = EventArgsAdapter.GetEventArgs<MarkerAddEventArgs>(applicationEvents, value);
					if (message is not null)
					{
						await AddMarkersAsync(message.newMarker);
					}
				});

			_eventHub.Subscribe(ApplicationEvents.OnResetMarker,
				async (applicationEvents, value) =>
				{
					var message = EventArgsAdapter.GetEventArgs<MarkerResetEventArgs>(applicationEvents, value);
					if (message is not null)
					{
						await ResetMarkersAsync();
					}
				});


		}


		public Task<List<Marker>> GetAllMarkersAsync()
		{
			return Task.FromResult(InMemoryMarker);
		}


		public Task<bool> AddMarkersAsync(Marker newMarker)
		{
			InMemoryMarker.Add(newMarker);
			var args = new MarkerAddedEventArgs(InMemoryMarker);
			_eventHub.Publish(ApplicationEvents.OnMarkerAdded, args);
			return Task.FromResult(true);
		}

		
		public Task ResetMarkersAsync()
		{
			InMemoryMarker.Clear();
			var args = new MarkerResetedEventArgs();
			_eventHub.Publish(ApplicationEvents.OnMarkerReseted, args);
			return Task.CompletedTask;
		}
	}
}