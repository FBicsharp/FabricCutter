using Blazored.Toast.Services;
using FabricCutter.UI.Components;
using System.Collections.Generic;

namespace FabricCutter.UI.Logic
{

	/// <summary>
	/// Rappresenta le infiormazioni relative alla ricetta 
	/// </summary>
	public class MarkersCommand : IMarkersCommand
	{

		public bool IsStartMarkerEnable { get; private set; }
		public bool IsEndMarkerEnable { get; private set; }
		public bool IsStartSubMarkerEnable { get; private set; }
		public bool IsEndSubMarkerEnable { get; private set; }
		public Marker? CurrentMarker { get; private set; }


		readonly ISlider _slider;
		readonly IMarkerFactory _markerFactory;
		private readonly IToastService toastService;

		public MarkersCommand(ISlider slider, IMarkerFactory markerFactory, IToastService toastService)
		{

			_slider = slider;
			_markerFactory = markerFactory;
			this.toastService = toastService;
			
		}

		public void StartMarker()
		{
			var newId = _slider.Markers.Any() ? _slider.Markers.Max(m => m.Id) : 1;
			_markerFactory.WithStartMarkerPosition(newId, _slider.PointerPosition);
		}

		public void EndMarker()
		{
			_markerFactory.WithStopMarkerPosition(_slider.PointerPosition);

		}

		public void EndSubMarker()
		{
			_markerFactory.WithStopSubMarkerPosition(_slider.PointerPosition);
		}

		public void StartSubMarker()
		{
			_markerFactory.WithStartSubMarkerPosition(_slider.PointerPosition);
		}

		public void FindMarkerSubMarker()
		{
			_markerFactory.WithStartSubMarkerPosition(_slider.PointerPosition);
		}


		/// <summary>
		/// trova se nella posizione corrente è presente un marker,
		/// </summary>
		public void FindMarkerAndEvalutePossibleAction()
		{
			CurrentMarker = _slider.Markers
				.OrderBy(m => m.Id)
				.FirstOrDefault(m =>
				   m.StartPosition < _slider.PointerPosition
				&& m.EndPosition > _slider.PointerPosition
				);

			if (CurrentMarker is null)
			{// nessun marker trovato qundi posso creare un nuovo marker 
				IsStartMarkerEnable = true;
				IsEndMarkerEnable = false;
				IsStartSubMarkerEnable = false;
				IsEndSubMarkerEnable = false;
				return;
			}

			//da questo id devo capire se ci sono altri marker tra lo start e lo stopo per capires e posso creare un nuovo marker al suo interno
			var exististSecondMarker = _slider.Markers
				.OrderBy(m => m.Id)
				.FirstOrDefault(m => m.Id > (CurrentMarker as Marker)?.Id
				&& m.StartPosition < _slider.PointerPosition
				&& m.EndPosition > _slider.PointerPosition);

			if (CurrentMarker.StartPosition>0 && CurrentMarker.EndPosition <= 0)
			{// posso chiudere il marker o reimpostare il nuovo punto di inizio
				IsStartMarkerEnable = true;
				IsEndMarkerEnable = true;
				IsStartSubMarkerEnable = false;
				IsEndSubMarkerEnable = false;
			}

			if (CurrentMarker.StartPosition >= 0 && CurrentMarker.EndPosition > 0)
			{// posso chiudere il marker o reimpostare il nuovo punto di inizio
				//e solo se sono a 50 dal primo marker posso creare un nuovo marker al suo interno
				IsStartMarkerEnable = exististSecondMarker is null ? true : false;
				IsEndMarkerEnable = exististSecondMarker is null ? true : false;
				IsStartSubMarkerEnable = false;
				IsEndSubMarkerEnable = false;

				if (CurrentMarker.SubMarker is null || exististSecondMarker?.SubMarker is null)
				{// posso chiudere il marker o reimpostare il nuovo punto di inizio
				 //e solo se sono a 50 dal primo marker posso creare un nuovo marker al suo interno
					
					IsStartSubMarkerEnable = true;
					IsEndSubMarkerEnable = true;
				}
			}





		}



	}

}
