using Blazored.Toast.Services;
using FabricCutter.UI.Components;
using FabricCutter.UI.Service;
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


		readonly IMarkerService _markerService;
		readonly IMarkerFactory _markerFactory;
		private readonly IToastService toastService;

		public MarkersCommand(IMarkerService markerService, IMarkerFactory markerFactory, IToastService toastService)
		{

			_markerService = markerService;
			_markerFactory = markerFactory;
			this.toastService = toastService;
			IsStartMarkerEnable = true;
			IsEndMarkerEnable = true;
			IsStartSubMarkerEnable = true;
			IsEndSubMarkerEnable = true;

		}

		public void StartMarker()
		{
			//_slider.AddMarker(new Logic.Marker(1, 700, 500)
			//{
			//	SubMarker = new Logic.SubMarker(1, 650, 550)
			//});
			//_slider.AddMarker(new Logic.Marker(2, 750, 550)
			//{
			//	SubMarker = new Logic.SubMarker(2, 700, 600)
			//});

			var newId = _markerService.Markers.Count>0 ? _markerService.Markers.Max(m => m.Id) : 1;
			_markerFactory.WithStartMarkerPosition(newId, _markerService.PointerPosition);
		}

		public void EndMarker()
		{
			_markerFactory.WithStopMarkerPosition(_markerService.PointerPosition);

		}

		public void EndSubMarker()
		{
			_markerFactory.WithStopSubMarkerPosition(_markerService.PointerPosition);
		}

		public void StartSubMarker()
		{
			_markerFactory.WithStartSubMarkerPosition(_markerService.PointerPosition);
		}

		public void FindMarkerSubMarker()
		{
			_markerFactory.WithStartSubMarkerPosition(_markerService.PointerPosition);
		}


		/// <summary>
		/// trova se nella posizione corrente è presente un marker,
		/// </summary>
		public void FindMarkerAndEvalutePossibleAction(object sender, ISlider slider)
		{
			CurrentMarker = _markerService.Markers
				.OrderBy(m => m.Id)
				.FirstOrDefault(m =>
				   m.StartPosition < _markerService.PointerPosition
				&& m.EndPosition > _markerService.PointerPosition
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
			var exististSecondMarker = _markerService.Markers
				.OrderBy(m => m.Id)
				.FirstOrDefault(m => m.Id > (CurrentMarker as Marker)?.Id
				&& m.StartPosition < _markerService.PointerPosition
				&& m.EndPosition > _markerService.PointerPosition);

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
