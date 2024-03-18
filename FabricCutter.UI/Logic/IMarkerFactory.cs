namespace FabricCutter.UI.Logic
{
	public interface IMarkerFactory
	{
		MarkerBase Build();
		void Clear();
		Marker WithStartMarkerPosition(int newMarkerId, int startPosition);
		Marker WithStartSubMarkerPosition(int startPosition);
		Marker WithStopMarkerPosition(int endPosition);
		Marker WithStopSubMarkerPosition(int endPosition);
	}
}