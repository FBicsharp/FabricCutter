namespace FabricCutter.UI.Logic
{
	public class Marker : MarkerBase, IMarkerBase
	{
		public SubMarker? SubMarker { get;  set; }
		

		public Marker(int id, int startPosition, int endPosition) 
			: base(id, startPosition, endPosition)
		{}

        
	}
}
