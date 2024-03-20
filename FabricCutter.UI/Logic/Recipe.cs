using FabricCutter.UI.Components;
using FabricCutter.UI.Service;
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
		public IMarkerService MarkerService { get; set; }

		public Recipe( 
			IRecipeDetailJsonFactory recipeDetailJsonFactory,
			IMarkersCommandViewModel markersCommand,
			IMarkerService markerService
		)
		{
			
			RecipeDetailJsonFactory = recipeDetailJsonFactory;
			MarkersCommand = markersCommand;
			MarkerService = markerService;
		}

		
	}
}
