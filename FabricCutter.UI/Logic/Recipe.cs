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
		public IRecipeDetailJsonFactory RecipeDetailJsonFactory { get; }
		public IMarkersCommandViewModel MarkersCommand { get; set; }

		public Recipe( IRecipeDetailJsonFactory recipeDetailJsonFactory, IMarkersCommandViewModel markersCommand)
		{
			
			RecipeDetailJsonFactory = recipeDetailJsonFactory;
			MarkersCommand = markersCommand;
			
		}

		
	}
}
