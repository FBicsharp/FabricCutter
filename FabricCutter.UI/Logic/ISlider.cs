
namespace FabricCutter.UI.Logic
{
	public interface ISlider
	{
		public List<Marker> Markers { get; }
		public int PointerPosition { get; set; }
		public int SliderLenght { get; set; }

		public void AddMarker(Marker newMarker);
		public void Reset();

		

	}
}