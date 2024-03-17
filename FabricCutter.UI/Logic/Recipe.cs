using FabricCutter.UI.Components;
using FabricCutter.UI.ViewModel;
using System.Collections.Generic;

namespace FabricCutter.UI.Logic
{

    /// <summary>
    /// Rappresenta le infiormazioni relative alla ricetta 
    /// </summary>
    public class Recipe : IRecipe
	{

		public IRecipeInformation RecipeInformation {get; set; }
		public ISlider Slider {get; set; }
		public IRecipeDetailJsonFactory RecipeDetailJsonFactory { get; }
		public IMarkersCommandViewModel MarkersCommand { get; set; }

		public Recipe(IRecipeInformation recipeInformation, ISlider slider, IRecipeDetailJsonFactory recipeDetailJsonFactory, IMarkersCommandViewModel markersCommand)
		{
			Slider = slider;			
			RecipeInformation = recipeInformation;
			RecipeDetailJsonFactory = recipeDetailJsonFactory;
			MarkersCommand = markersCommand;
			
		}

		
	}
}
