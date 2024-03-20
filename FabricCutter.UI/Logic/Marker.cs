using Newtonsoft.Json;

namespace FabricCutter.UI.Logic
{
	public class Marker : MarkerBase, IMarkerBase
	{
		[JsonProperty("Index")]
		public override int Id { get => base.Id; set => base.Id = value; }
		[JsonProperty("Splices")]
		public List<SubMarker>? SubMarker { get;  set; }
		

		public Marker(int id, int startPosition, int endPosition) 
			: base(id, startPosition, endPosition)
		{}

        
	}
}
