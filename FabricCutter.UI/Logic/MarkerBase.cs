namespace FabricCutter.UI.Logic
{
	public abstract class MarkerBase : IMarkerBase
	{


		public virtual int Id { get; set; }
		public virtual int StartPosition { get; set; }
        public virtual int EndPosition { get; set; }
		public virtual int MarkerLenght => (EndPosition -StartPosition);

		public MarkerBase(int markerId, int startPosition, int endPosition)
		{
			Id = markerId;
			StartPosition = startPosition;
			EndPosition = endPosition;
		}
	}
}
