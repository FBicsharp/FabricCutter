namespace FabricCutter.UI.Logic
{
	public interface IMarkerFactory
	{
		MarkerBase Build();
		bool WithStartMarkerPosition(int newMarkerId, int startPosition);
		bool WithStartSubMarkerPosition(int startPosition);
		bool WithStopMarkerPosition(int endPosition);
		bool WithStopSubMarkerPosition(int endPosition);
	}
}