namespace FabricCutter.UI.Logic
{
	public interface IMarkerBase
	{
		int EndPosition { get; set; }
		int MarkerLenght { get; }
		int StartPosition { get; set; }
	}
}