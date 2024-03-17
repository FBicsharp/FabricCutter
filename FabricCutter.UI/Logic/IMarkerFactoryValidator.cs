namespace FabricCutter.UI.Logic
{
	public interface IMarkerFactoryValidator
	{
		bool IsEndMarkerEnable { get; set; }
		bool IsEndSubMarkerEnable { get; set; }
		bool IsStartMarkerEnable { get; set; }
		bool IsStartSubMarkerEnable { get; set; }
		bool IsValid { get; set; }

		MarkerFactoryValidator Validate(Marker marker);
	}
}