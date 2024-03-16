namespace FabricCutter.UI.Logic
{
	public class Slider 
	{
        public List<Marker> _markers { get; protected set; }		
		public int PointerPosition { get; set; }
		public int SliderLenght { get; set; }


		public Slider(int sliderLenght)
		{
			_markers = new ();					
			
			SliderLenght = sliderLenght;
			PointerPosition = 0;
		}

		public void AddMarker(Marker newMarker)
		{
			_markers.Add(newMarker);			
		}
		public void Reset() => _markers.Clear();





	}
}
