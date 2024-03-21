using Newtonsoft.Json;


namespace FabricCutter.UI.Logic
{
	public abstract class MarkerBase : IMarkerBase
	{		
		public virtual int Id { get; set; }
		[JsonProperty("Start", Order = 3)]
		public virtual int StartPosition { get; set; }
		[JsonProperty("Stop", Order = 4)]
		public virtual int EndPosition { get; set; }
		[JsonIgnore]
		public virtual int MarkerLenght => (EndPosition -StartPosition);

		public MarkerBase(int markerId, int startPosition, int endPosition)
		{
			Id = markerId;
			StartPosition = startPosition;
			EndPosition = endPosition;
		}
	}
}
