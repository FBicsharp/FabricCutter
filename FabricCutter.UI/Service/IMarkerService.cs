using FabricCutter.UI.Logic;

namespace FabricCutter.UI.Service
{
	public interface IMarkerService
	{
		Task<List<Marker>> GetAllMarkersAsync();
		Task<bool> AddMarkersAsync(Marker newMarker);
		Task ResetMarkersAsync();

	}
}