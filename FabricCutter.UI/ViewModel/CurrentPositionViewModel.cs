using FabricCutter.UI.Logic.Events;


namespace FabricCutter.UI.ViewModel
{
	public class CurrentPositionViewModel : ICurrentPositionViewModel
	{
		public int currentPosition { get; set; }

		private readonly IEventHub _eventHub;

		public Action StateHasChanged { get; set; } = () => { };
		public CurrentPositionViewModel(
			IEventHub eventHub
			)
		{
			_eventHub = eventHub;
			_eventHub.Subscribe(ApplicationEvents.OnPointerPositionChanged, OnPointerPositionChanged);
		}


		private void OnPointerPositionChanged(ApplicationEvents applicationEvents, object value)
		{
			var message = EventArgsAdapter.GetEventArgs<PointerPositionChangedEventArgs>(applicationEvents, value);
			if (message is not null)
			{
				currentPosition = message.pointerPosition;
				StateHasChanged();
			}
		}

		public void Dispose()
		{
			_eventHub.Unsubscribe(ApplicationEvents.OnPointerPositionChanged, OnPointerPositionChanged);
		}
	}
}
