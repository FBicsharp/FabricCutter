using FabricCutter.UI.Logic;

namespace FabricCutter.UI.ViewModel
{
    public interface IRecipeInformationViewModel : IViewModel
	{
		int TotalLenght { get;  set; }
		int MarkersLenght { get; set; }
		int SplicesNumber { get; set; }
		int MarkersNumber { get; set; }
		int AbsolutePosition { get; set; }

	}
}