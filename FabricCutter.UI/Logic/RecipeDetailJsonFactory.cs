using FabricCutter.UI.Components;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FabricCutter.UI.Logic
{

	/// <summary>
	/// Rappresenta le infiormazioni relative alla ricetta 
	/// </summary>
	public class RecipeDetailJsonFactory
	{
		
		private RecipeInformation RecipeInformation { get; }

		public RecipeDetailJsonFactory(RecipeInformation recipeInformation)
		{
			RecipeInformation = recipeInformation;
		}


		public string Create()
		{
			var recipeDetail = new RecipeDetail();
			recipeDetail.Name = "Ricetta";
			recipeDetail.CreationDate = DateTime.UtcNow;
			recipeDetail.TotalLenght = RecipeInformation.TotalLenght;
			recipeDetail.MarkersLenght = RecipeInformation.MarkersLenght;
			recipeDetail.SplicesNumber = RecipeInformation.SplicesNumber;
			recipeDetail.MarkersNumber = RecipeInformation.MarkersNumber;
			recipeDetail.AbsolutePosition = RecipeInformation.AbsolutePosition;
			recipeDetail.Markers = RecipeInformation.Markers;
			return JsonConvert.SerializeObject(recipeDetail);
			
		}


	}
}
