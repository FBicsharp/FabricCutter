using Newtonsoft.Json;

namespace FabricCutter.UI.Logic
{
	public class SubMarker : MarkerBase, IMarkerBase
	{
		[JsonProperty("MarkerIndex", Order = 1)]
		public override int Id { get => base.Id; set => base.Id= value; }
				
		public SubMarker(int markerId,int startPosition, int endPosition) 
			: base(markerId,startPosition, endPosition)
		{}
	}
}
