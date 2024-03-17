
namespace FabricCutter.UI.Logic
{
	public interface IRecipeInformation
	{
		int AbsolutePosition { get; }
		List<Marker> Markers { get; }
		int MarkersLenght { get; }
		int MarkersNumber { get; }
		int SplicesNumber { get; }
		int TotalLenght { get; }
		void EvalutateMarkers(List<Marker> markers);
	}
}