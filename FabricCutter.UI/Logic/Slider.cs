using FabricCutter.UI.Service;

namespace FabricCutter.UI.Logic
{
	public class Slider : ISlider
	{
		public List<Marker> Markers { get; protected set; }
		public int PointerPosition { get; set; }
		public int SliderLenght { get; set; }


		public Slider(ApplicationSettings applicationSettings)
		{
			Markers = new();
			SliderLenght = applicationSettings.SliderLenght;
			PointerPosition = 0;
		}

		public void AddMarker(Marker newMarker)
		{
			Markers.Add(newMarker);
		}
		public void Reset() => Markers.Clear();





	}
}
