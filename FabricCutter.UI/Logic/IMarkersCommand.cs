namespace FabricCutter.UI.Logic
{
	public interface IMarkersCommand
	{
		bool IsStartMarkerEnable { get; }
		bool IsEndMarkerEnable { get; }
		bool IsEndSubMarkerEnable { get; }
		bool IsStartSubMarkerEnable { get; }


		void StartMarker();
		void EndMarker();
		void EndSubMarker();
		void StartSubMarker();


	}
}