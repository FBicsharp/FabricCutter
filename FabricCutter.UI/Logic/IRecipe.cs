
using FabricCutter.UI.ViewModel;

namespace FabricCutter.UI.Logic
{
    public interface IRecipe
	{
		public IMarkersCommandViewModel MarkersCommand { get; set; }
		public IRecipeDetailJsonFactory RecipeDetailJsonFactory { get; }
		
	}
}