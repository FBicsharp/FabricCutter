
namespace FabricCutter.UI.Logic
{
	public interface IRecipe
	{

		public IRecipeInformation RecipeInformation { get; set; }
		public ISlider Slider { get; set; }
		public IMarkersCommand MarkersCommand { get; set; }
		public IRecipeDetailJsonFactory RecipeDetailJsonFactory { get; }
		
	}
}