namespace FabricCutter.UI.Logic
{
	public interface IMarkerFactory
	{
		Marker BuildMarker();
		void Clear();
		void WithStartMarkerPosition(int newMarkerId, int startPosition);
		void WithStartSubMarkerPosition( int startPosition);
		void WithStopMarkerPosition(int endPosition);
		void WithStopSubMarkerPosition(int endPosition);
	}
}