using Blazored.Toast.Services;
using FabricCutter.UI.Logic;
using FabricCutter.UI.Service;
using Microsoft.JSInterop;

namespace FabricCutter.UI.ViewModel
{
    public class RecipeViewModel : IRecipeViewModel
	{
		
		private readonly IToastService _toastService;
		private readonly IJSRuntime _jSRuntime;
		public IRecipe Recipe { get; }
		

		public Action StateHasChanged { get; set; }= () => { };

		public RecipeViewModel(
			IToastService toastService, 
			IJSRuntime jSRuntime,
			IRecipe recipe
			)
        {   
			_toastService = toastService;
			_jSRuntime = jSRuntime;
			Recipe = recipe;
			
		}

		public void Dispose()
		{
			
		}
	}
}
