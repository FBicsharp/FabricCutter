using FabricCutter.UI.Logic;
using FabricCutter.UI.Logic.Events;
using FabricCutter.UI.Service;


namespace FabricCutter.UI.ViewModel
{
	public class RecipeInformationViewModel : IRecipeInformationViewModel
	{
		public int currentPosition { get; set; }

		private readonly IEventHub _eventHub;
		private readonly ApplicationSettings applicationSettings;

		public Action StateHasChanged { get; set; } = () => { };
		public int TotalLenght { get;  set; }
		public int MarkersLenght { get;  set; }
		public int SplicesNumber { get;  set; }
		public int MarkersNumber { get;  set; }
		public int AbsolutePosition { get;  set; }

		public RecipeInformationViewModel(
			IEventHub eventHub,
			ApplicationSettings applicationSettings
		)
		{
			_eventHub = eventHub;
			this.applicationSettings = applicationSettings;
			InitializeSubscribeEvents();
		}


		

		#region EVENTS




		private void InitializeSubscribeEvents()
		{
			_eventHub.Subscribe(ApplicationEvents.OnPointerPositionChanged, OnPointerPostionChangeHandler);
			_eventHub.Subscribe(ApplicationEvents.OnMarkerAdded, OnMarkerAddedHandler);
			_eventHub.Subscribe(ApplicationEvents.OnMarkerUpdated, OnMarkerUpdatedHandler);
			_eventHub.Subscribe(ApplicationEvents.OnResetMarker, OnResetMarkerHandler);
		}
		private void UnsubscribeEvents()
		{
			_eventHub.Unsubscribe(ApplicationEvents.OnPointerPositionChanged, OnPointerPostionChangeHandler);
			_eventHub.Unsubscribe(ApplicationEvents.OnMarkerAdded, OnMarkerAddedHandler);
			_eventHub.Unsubscribe(ApplicationEvents.OnMarkerUpdated, OnMarkerUpdatedHandler);
			_eventHub.Unsubscribe(ApplicationEvents.OnResetMarker, OnResetMarkerHandler);
		}



		private void OnMarkerAddedHandler(ApplicationEvents applicationEvents, object value)
		{
			var markersList = EventArgsAdapter.GetEventArgs<MarkerAddedEventArgs>(applicationEvents, value)
				.markersList
				.AsEnumerable<IMarkerBase>()
				.ToList();
			EvalutateMarkers(markersList);
			StateHasChanged();
		}
		private void OnMarkerUpdatedHandler(ApplicationEvents applicationEvents, object value)
		{
			var markersList = EventArgsAdapter.GetEventArgs<MarkerUpdatedEventArgs>(applicationEvents, value)
				.markersList
				.AsEnumerable<IMarkerBase>()
				.ToList(); ;
			EvalutateMarkers(markersList);
			StateHasChanged();
		}



		private void OnResetMarkerHandler(ApplicationEvents applicationEvents, object value)
		{
			TotalLenght = 0;
			MarkersLenght = 0;
			SplicesNumber = 0;
			MarkersNumber = 0;
			StateHasChanged();
		}

		private void OnPointerPostionChangeHandler(ApplicationEvents applicationEvents, object value)
		{
			AbsolutePosition = applicationSettings.SliderLenght - EventArgsAdapter.GetEventArgs<PointerPositionChangedEventArgs>(applicationEvents, value).pointerPosition;			
			StateHasChanged();
		}

		#endregion







		public void EvalutateMarkers(List<IMarkerBase> markers)
		{
			markers= markers.Except(markers.Where(m => m.EndPosition < 0)).ToList();//excludo quelli ancora non definiti
			var cnt = markers.Count();
			var tmpLenght = cnt > 0 ? (markers.Max(m => m.StartPosition) - markers.Min(m => m.EndPosition)) : 0;
			TotalLenght = tmpLenght < 0 ? 0 : tmpLenght;
			MarkersLenght = cnt > 0 ? markers.Where(m=>m.EndPosition>=0).Sum(m => m.MarkerLenght) : 0;
			SplicesNumber = markers.Where(c=> c is SubMarker).Count();
			MarkersNumber = markers.Where(c => c is Marker).Count();			
		}


		public void Dispose()
		{
			UnsubscribeEvents();
		}
	}
}
