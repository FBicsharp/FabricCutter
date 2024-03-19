using FabricCutter.UI.Logic.Events;
using FabricCutter.UI.Service;
using FabricCutter.UI.ViewModel;
using Microsoft.AspNetCore.Components;
using System.Runtime;

namespace FabricCutter.UI.Logic
{
	public class CursorPositionViewModel : ICursorPositionViewModel
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly IEventHub _eventHub;


		private int PointerPosition { get; set; }
		public string CursorClassStyle { get; set; } = "cursor-position-delimitator cursor-position-start";
		public string CursorStyle { get; set; }
		public Action StateHasChanged { get; set; } = () => { };

		public CursorPositionViewModel(ApplicationSettings applicationSettings,
			IEventHub eventHub
		)
		{
			PointerPosition = 0;
			_applicationSettings = applicationSettings;
			_eventHub = eventHub;
			SubscribeEvents();
		}


		private void OnStartOrEndPointerPositionChangedHandler(ApplicationEvents applicationEvents, object value)
		{
			var message = EventArgsAdapter.GetEventArgs<PointerPositionStartOrEndEventArgs>(applicationEvents, value);
			if (message is not null)
			{	
				if (message.IsEnd)
					CursorClassStyle = "cursor-position-delimitator cursor-position-end";
				else if (message.IsStart)
					CursorClassStyle = "cursor-position-delimitator cursor-position-start";
				else
					CursorClassStyle = "cursor-position-delimitator cursor-position-start";

				StateHasChanged();
			}
		}

		private void OnPointerPositionChangedHandler(ApplicationEvents applicationEvents, object value)
		{
			var message = EventArgsAdapter.GetEventArgs<PointerPositionChangedEventArgs>(applicationEvents, value);
			if (message is not null)
			{
				PointerPosition = message.pointerPosition;

				int maxMargin = -15; // Massimo margine
				int minMargin = 10; // Minimo margine

				// Calcolo del margine
				int calculatedMargin = (int)(((PointerPosition / (double)_applicationSettings.SliderLenght) * (maxMargin - minMargin)) + minMargin);

				

				CursorStyle = $"z-index:{1000 }; " +
					$"left: calc(({PointerPosition} / {_applicationSettings.SliderLenght}) * 100%); " +
					$"margin-left: {calculatedMargin}px;";
				StateHasChanged();
			}
		}



		private void SubscribeEvents()
		{
			_eventHub.Subscribe(ApplicationEvents.OnPointerPositionStartOrEndChanged, OnStartOrEndPointerPositionChangedHandler);
			_eventHub.Subscribe(ApplicationEvents.OnPointerPositionChanged, OnPointerPositionChangedHandler);
		}

		private void UnsubscribeEvents()
		{
			_eventHub.Unsubscribe(ApplicationEvents.OnPointerPositionStartOrEndChanged, OnStartOrEndPointerPositionChangedHandler);
			_eventHub.Unsubscribe(ApplicationEvents.OnPointerPositionChanged, OnPointerPositionChangedHandler);

		}

		public void Dispose()
		{
			UnsubscribeEvents();
		}
	}
}
