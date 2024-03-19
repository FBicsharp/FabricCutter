using FabricCutter.UI.Logic;
using FabricCutter.UI.Service;

namespace FabricCutter.UI.ViewModel
{
    public interface IRecipeViewModel : IViewModel
	{	
		IRecipe Recipe { get; }		

	}
}