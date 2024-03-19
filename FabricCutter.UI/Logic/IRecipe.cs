﻿
using FabricCutter.UI.Service;
using FabricCutter.UI.ViewModel;

namespace FabricCutter.UI.Logic
{
    public interface IRecipe
	{
		IMarkerService MarkerService { get; set; }
		IMarkersCommandViewModel MarkersCommand { get; set; }
		IRecipeDetailJsonFactory RecipeDetailJsonFactory { get; }
		
	}
}