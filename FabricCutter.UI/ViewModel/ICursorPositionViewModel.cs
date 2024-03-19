
using FabricCutter.UI.ViewModel;

namespace FabricCutter.UI.Logic
{
	public interface ICursorPositionViewModel :IViewModel
	{
		string CursorClassStyle { get; set; }
		string CursorStyle { get; set; }
		Action StateHasChanged { get; set; }

		void Dispose();
	}
}