using Blazored.Toast.Services;
using FabricCutter.UI.Components;
using FabricCutter.UI.Logic;
using FabricCutter.UI.Logic.Events;
using FabricCutter.UI.Service;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace FabricCutter.UI.ViewModel
{

	/// <summary>
	/// Rappresenta le infiormazioni relative alla ricetta 
	/// </summary>
	public class MarkersCommandViewModel : IMarkersCommandViewModel
	{

		public bool IsStartMarkerEnable { get; private set; }
		public bool IsEndMarkerEnable { get; private set; }
		public bool IsStartSubMarkerEnable { get; private set; }
		public bool IsEndSubMarkerEnable { get; private set; }


		public Marker? MarkerInEditingMode { get; private set; }
		public Marker? ClosestMarkerFromCurretnPosition { get; private set; }
		public Marker? SecondMarkerInCurrentPosition { get; private set; }

		private int PointerPosition { get; set; }
		private List<Marker> Markers { get; set; }
		public Action StateHasChanged { get; set; } = () => { };

		private readonly IEventHub _eventHub;
		readonly IMarkerFactory _markerFactory;
		private readonly IToastService toastService;

		public MarkersCommandViewModel(
			IEventHub eventHub, IMarkerFactory markerFactory, IToastService toastService
			)
		{
			_eventHub = eventHub;
			Markers = new();
			_markerFactory = markerFactory;
			this.toastService = toastService;
			IsStartMarkerEnable = true;
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
			Markers = EventArgsAdapter.GetEventArgs<MarkerAddedEventArgs>(applicationEvents, value).markersList;
			EvalutePossibleAction();
		}
		private void OnMarkerUpdatedHandler(ApplicationEvents applicationEvents, object value)
		{
			Markers = EventArgsAdapter.GetEventArgs<MarkerUpdatedEventArgs>(applicationEvents, value).markersList;
			EvalutePossibleAction();
		}



		private void OnResetMarkerHandler(ApplicationEvents applicationEvents, object value)
		{
			_markerFactory.Clear();
			Markers.Clear();
			MarkerInEditingMode = null;
			EvalutePossibleAction();
		}

		private void OnPointerPostionChangeHandler(ApplicationEvents applicationEvents, object value)
		{
			PointerPosition = EventArgsAdapter.GetEventArgs<PointerPositionChangedEventArgs>(applicationEvents, value).pointerPosition;
			EvalutePossibleAction();
		}

		#endregion


		public void StartMarker()
		{

			var newId = Markers.Count > 0 ? Markers.Max(m => m.Id) + 1 : 1;
			_markerFactory.WithStartMarkerPosition(newId, PointerPosition);
			if (MarkerInEditingMode is not null)
			{
				MarkerInEditingMode.StartPosition = PointerPosition;
				var update_args = new MarkerUpdateEventArgs(MarkerInEditingMode);
				_eventHub.Publish(ApplicationEvents.OnUpdateMarker, update_args);
				return;
			}
			MarkerInEditingMode = _markerFactory.BuildMarker();
			var args = new MarkerAddEventArgs(MarkerInEditingMode);
			_eventHub.Publish(ApplicationEvents.OnAddMarker, args);
			var args2 = new PointerPositionStartOrEndEventArgs(true, false);
			_eventHub.Publish(ApplicationEvents.OnPointerPositionStartOrEndChanged, args2);

		}

		public void EndMarker()
		{		
			if (MarkerInEditingMode is null)
			{
				toastService.ShowWarning("Marcatore non trovato");
				return;
			}
			_markerFactory.WithStopMarkerPosition(PointerPosition);
			MarkerInEditingMode = _markerFactory.BuildMarker() ;
			if (MarkerInEditingMode.EndPosition == int.MinValue)
			{
				toastService.ShowWarning("La fine deve essere dopo l'inizio");
				return;
			}
			var args = new MarkerUpdateEventArgs(MarkerInEditingMode);
			_eventHub.Publish(ApplicationEvents.OnUpdateMarker, args);
			MarkerInEditingMode = null;
			_markerFactory.Clear();
			var args2 = new PointerPositionStartOrEndEventArgs(false, true);
			_eventHub.Publish(ApplicationEvents.OnPointerPositionStartOrEndChanged, args2);
		}


		public void StartSubMarker()
		{
			if (MarkerInEditingMode is null )
				return;			
			_markerFactory.WithStartSubMarkerPosition(PointerPosition);
			MarkerInEditingMode = _markerFactory.BuildMarker() ;
			var args = new MarkerUpdateEventArgs(MarkerInEditingMode);
			_eventHub.Publish(ApplicationEvents.OnUpdateMarker, args);
		}
		public void EndSubMarker()
		{
			if (MarkerInEditingMode is null)
			{
				toastService.ShowWarning("Marcatore non trovato");
				return;
			}
			_markerFactory.WithStopSubMarkerPosition(PointerPosition);

			if (MarkerInEditingMode.SubMarker.Where(s=>s.EndPosition == int.MinValue).Any())
			{
				toastService.ShowWarning("La fine deve essere dopo l'inizio");
				return;
			}			
			MarkerInEditingMode = _markerFactory.BuildMarker() ;
			var args = new MarkerUpdateEventArgs(MarkerInEditingMode);
			_eventHub.Publish(ApplicationEvents.OnUpdateMarker, args);			
		}

		#region BUTTON CHECKS


		public void EvalutePossibleAction()
		{
			IsStartSubMarkerEnable = true;
			IsEndSubMarkerEnable = true;
			IsStartMarkerEnable = true;
			IsEndMarkerEnable = true;
			StateHasChanged();

		}






		#endregion

		public void Dispose()
		{
			UnsubscribeEvents();
		}
	}

}
