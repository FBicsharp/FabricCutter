using FabricCutter.UI.Logic.Events;


namespace FabricCutter.UI.ViewModel
{
	public class ReciperCommandViewModel : IReciperCommandViewModel
	{
		public int currentPosition { get; set; }

		private readonly IEventHub _eventHub;

		public Action StateHasChanged { get; set; } = () => { };
		public ReciperCommandViewModel(
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

		public void OnConfirm()
		{
			_eventHub.Publish(ApplicationEvents.OnExportRecipe, new MarkerExportEventArgs());
		}

		public void OnReset()
		{
			_eventHub.Publish(ApplicationEvents.OnPointerPositionStartOrEndChanged, new PointerPositionStartOrEndEventArgs());
			_eventHub.Publish(ApplicationEvents.OnResetMarker, new MarkerResetEventArgs());
		}
	}
}
