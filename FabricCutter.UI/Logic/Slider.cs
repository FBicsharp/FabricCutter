using FabricCutter.UI.Logic.Events;
using FabricCutter.UI.Service;

namespace FabricCutter.UI.Logic
{
	public class Slider : ISlider
	{
		private readonly IEventHub _eventHub;
		private readonly IMarkerService _markerService;

		public List<Marker> Markers { get; protected set; }
		public int PointerPosition { get; set; }
		public int SliderLenght { get; set; }


		public Slider(ApplicationSettings applicationSettings,
			IEventHub eventHub,
			IMarkerService markerService
			)
		{
			Markers = new();
			SliderLenght = applicationSettings.SliderLenght;
			PointerPosition = 0;
			_eventHub = eventHub;
			_markerService = markerService;
		}

		public void AddMarker(Marker newMarker)
		{
			Markers.Add(newMarker);
			

		}
		public void Reset() => Markers.Clear();

		
		










	}
}
