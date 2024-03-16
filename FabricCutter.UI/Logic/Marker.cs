namespace FabricCutter.UI.Logic
{
	public class Marker : MarkerBase
	{
		public SubMarker? SubMarker { get;  set; }
		public int Id { get;  private set; }

		public Marker(int id, int startPosition, int endPosition) 
			: base( startPosition, endPosition)
		{
			Id = id;
		}

        
	}
}
