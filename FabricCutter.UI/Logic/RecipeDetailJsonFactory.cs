using FabricCutter.UI.Components;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FabricCutter.UI.Logic
{

	/// <summary>
	/// Rappresenta le infiormazioni relative alla ricetta 
	/// </summary>
	public class RecipeDetailJsonFactory : IRecipeDetailJsonFactory
	{
		

		public string Create(List<IMarkerBase> markerList)
		{
			
			return JsonConvert.SerializeObject(markerList);

		}


	}
}
