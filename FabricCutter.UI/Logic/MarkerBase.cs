namespace FabricCutter.UI.Logic
{
	public abstract class MarkerBase
	{

        
        public virtual  int StartPosition { get;  set; }
        public virtual  int EndPosition { get;  set; }
        public virtual int MarkerLenght => (EndPosition - StartPosition);

		public MarkerBase( int startPosition, int endPosition)
		{
			StartPosition = startPosition;
			EndPosition = endPosition;
		}
	}
}
