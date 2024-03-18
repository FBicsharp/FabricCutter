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
            IsEndMarkerEnable = true;
            IsStartSubMarkerEnable = true;
            IsEndSubMarkerEnable = true;
			InitializeSubscribeEvents();

		}






		private void InitializeSubscribeEvents()
		{
			_eventHub.Subscribe(ApplicationEvents.OnPointerPositionChanged,
                (applicationEvents, value) 
                => PointerPosition = EventArgsAdapter.GetEventArgs<PointerPositionChangedEventArgs>(applicationEvents, value).pointerPosition
				//gestisco il cambi odi posizione per trovare quale elemento è su questa posizione
				);

            _eventHub.Subscribe(ApplicationEvents.OnAddMarker, null);
                //aggiorna la mia lista di marker e poi //gestisco il cambi odi posizione per trovare quale elemento è su questa posizione
                //infine imposta a null quello corretne perche è stato aggiunto


            _eventHub.Subscribe(ApplicationEvents.OnResetMarker, null);
            //resetta la lista di marker corrente e riabilita i pulsanti


		}

		private void UnsubscribeEvents()
		{
			_eventHub.Unsubscribe(ApplicationEvents.OnPointerPositionChanged,
				(applicationEvents, value)
				=> PointerPosition = EventArgsAdapter.GetEventArgs<PointerPositionChangedEventArgs>(applicationEvents, value).pointerPosition
				//gestisco il cambi odi posizione per trovare quale elemento è su questa posizione
				);

			_eventHub.Unsubscribe(ApplicationEvents.OnAddMarker, null);
			//aggiorna la mia lista di marker e poi //gestisco il cambi odi posizione per trovare quale elemento è su questa posizione
			//infine imposta a null quello corretne perche è stato aggiunto


			_eventHub.Unsubscribe(ApplicationEvents.OnResetMarker, null);
			//resetta la lista di marker corrente e riabilita i pulsanti


		}



		public void StartMarker()
        {

			var newId = Markers.Count > 0 ? Markers.Max(m => m.Id) : 1;
            var newmarker   =_markerFactory.WithStartMarkerPosition(newId, PointerPosition);

			

			var args = new MarkerAddEventArgs(newmarker);
			_eventHub.Publish(ApplicationEvents.OnAddMarker, args);
        }

        public void EndMarker()
        {
			var newmarker  = _markerFactory.WithStopMarkerPosition(PointerPosition);
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
        public void FindMarkerAndEvalutePossibleAction(object sender, ISlider slider)
        {
            FirstMarkerInCurrentPosition = Markers
                .OrderBy(m => m.Id)
                .FirstOrDefault(m =>
                   m.StartPosition < PointerPosition
                && m.EndPosition > PointerPosition
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
                .FirstOrDefault(m => m.Id > (FirstMarkerInCurrentPosition as Marker)?.Id
                && m.StartPosition < PointerPosition
                && m.EndPosition > PointerPosition);

            if (FirstMarkerInCurrentPosition.StartPosition > 0 && FirstMarkerInCurrentPosition.EndPosition <= 0)
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





        }

		public void Dispose()
		{
            UnsubscribeEvents();
		}
	}

}
