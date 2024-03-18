using FabricCutter.UI.Logic;

namespace FabricCutter.UI.ViewModel
{
    public interface IRecipeViewModel : IViewModel
	{	
		IRecipe Recipe { get; }

	}
}