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


		public Marker? FirstMarkerInCurrentPosition { get; private set; }
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

			var newId = Markers.Count > 0 ? Markers.Max(m => m.Id) : 1;
			var newmarker = _markerFactory.WithStartMarkerPosition(newId, PointerPosition);
			var sameMarker = Markers.FirstOrDefault(m => m.Id == newmarker.Id);
			if (sameMarker is not null)
			{
				sameMarker.StartPosition = PointerPosition;
				var update_args = new MarkerUpdateEventArgs(newmarker);
				_eventHub.Publish(ApplicationEvents.OnUpdateMarker, update_args);
				return;
			}

			var args = new MarkerAddEventArgs(newmarker);
			_eventHub.Publish(ApplicationEvents.OnAddMarker, args);
		}

		public void EndMarker()
		{
			var newmarker = _markerFactory.WithStopMarkerPosition(PointerPosition);
			if (newmarker.EndPosition == int.MinValue)
			{
				toastService.ShowWarning("La fine deve essere dopo l'inizio");				
			}
			var args = new MarkerUpdateEventArgs(newmarker);
			_eventHub.Publish(ApplicationEvents.OnUpdateMarker, args);

		}


		public void StartSubMarker()
		{
			var newmarker = _markerFactory.WithStartSubMarkerPosition(PointerPosition);
			var args = new MarkerUpdateEventArgs(newmarker);
			_eventHub.Publish(ApplicationEvents.OnUpdateMarker, args);
		}
		public void EndSubMarker()
		{
			var newmarker = _markerFactory.WithStopSubMarkerPosition(PointerPosition);
			var args = new MarkerUpdateEventArgs(newmarker);
			_eventHub.Publish(ApplicationEvents.OnUpdateMarker, args);
		}



		/// <summary>
		/// trova se nella posizione corrente è presente un marker,
		/// </summary>
		public void EvalutePossibleAction()
		{
			FirstMarkerInCurrentPosition = Markers
				.OrderBy(m => m.Id)
				.FirstOrDefault(m =>
				   m.StartPosition <= PointerPosition
				&&  PointerPosition <= (m.EndPosition== int.MinValue?int.MaxValue: m.EndPosition)//filtra i marker che non hanno una posizione di fine
				);

			if (FirstMarkerInCurrentPosition is null)
			{// nessun marker trovato qundi posso creare un nuovo marker 
				IsStartMarkerEnable = true;
				IsEndMarkerEnable = false;
				IsStartSubMarkerEnable = false;
				IsEndSubMarkerEnable = false;
				return;
			}

			//da questo id devo capire se ci sono altri marker tra lo start e lo stopo per capires e posso creare un nuovo marker al suo interno
			var exististSecondMarker = Markers
				.OrderBy(m => m.Id)
				.FirstOrDefault(m => m.Id != (FirstMarkerInCurrentPosition as Marker)?.Id
				&& m.StartPosition >= FirstMarkerInCurrentPosition.StartPosition
				&& m.EndPosition >= PointerPosition);

			if (FirstMarkerInCurrentPosition.StartPosition >= 0 && FirstMarkerInCurrentPosition.EndPosition <= 0)
			{// posso chiudere il marker o reimpostare il nuovo punto di inizio
				IsStartMarkerEnable = true;
				IsEndMarkerEnable = true;
				IsStartSubMarkerEnable = false;
				IsEndSubMarkerEnable = false;
			}

			if (FirstMarkerInCurrentPosition.StartPosition >= 0 && FirstMarkerInCurrentPosition.EndPosition > 0)
			{// posso chiudere il marker o reimpostare il nuovo punto di inizio
			 //e solo se sono a 50 dal primo marker posso creare un nuovo marker al suo interno
				IsStartMarkerEnable = exististSecondMarker is null ? true : false;
				IsEndMarkerEnable = exististSecondMarker is null ? true : false;
				IsStartSubMarkerEnable = false;
				IsEndSubMarkerEnable = false;

				if (FirstMarkerInCurrentPosition.SubMarker is null || exististSecondMarker?.SubMarker is null)
				{// posso chiudere il marker o reimpostare il nuovo punto di inizio
				 //e solo se sono a 50 dal primo marker posso creare un nuovo marker al suo interno

					IsStartSubMarkerEnable = true;
					IsEndSubMarkerEnable = true;
				}
			}
			StateHasChanged();





		}

		public void Dispose()
		{
			UnsubscribeEvents();
		}
	}

}
