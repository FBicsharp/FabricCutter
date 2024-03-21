using Newtonsoft.Json;

namespace FabricCutter.UI.Logic
{
	public class Marker : MarkerBase, IMarkerBase
	{
		[JsonProperty("Index", Order = 1)]		
		public override int Id { get => base.Id; set => base.Id = value; }

		[JsonProperty("Offset", Order = 2)]
		public int Offset { get; set; } 


		[JsonProperty("Splices",Order = 5)]
		public List<SubMarker>? SubMarker { get;  set; }
		

		public Marker(int id, int startPosition, int endPosition) 
			: base(id, startPosition, endPosition)
		{
			SubMarker = new List<SubMarker>();
			Offset = 50;
		}

        
	}
}
