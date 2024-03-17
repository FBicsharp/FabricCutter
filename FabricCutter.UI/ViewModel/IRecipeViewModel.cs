using FabricCutter.UI.Logic;

namespace FabricCutter.UI.ViewModel
{
    public interface IRecipeViewModel
	{
		
		public Action StateHasChenged { get; set; }
		IRecipe Recipe { get; }

		public void OnAddNewMarkerComplete(Marker newMarker);


	}
}