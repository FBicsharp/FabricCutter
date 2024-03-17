using FabricCutter.UI.Components;
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
		public IMarkersCommand MarkersCommand { get; set; }

		public Recipe(IRecipeInformation recipeInformation, ISlider slider, IRecipeDetailJsonFactory recipeDetailJsonFactory, IMarkersCommand markersCommand)
		{
			RecipeInformation = recipeInformation;
			Slider = slider;
			RecipeDetailJsonFactory = recipeDetailJsonFactory;
			MarkersCommand = markersCommand;
		}

		
	}
}
