using Newtonsoft.Json;


namespace FabricCutter.UI.Logic
{
	public abstract class MarkerBase : IMarkerBase
	{

		[JsonIgnore]
		public virtual int Id { get; set; }
		[JsonProperty("Start")]
		public virtual int StartPosition { get; set; }
		[JsonProperty("Stop")]
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
