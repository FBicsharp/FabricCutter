using Blazored.Toast.Services;
using FabricCutter.UI.Logic;
using FabricCutter.UI.Logic.Events;
using FabricCutter.UI.Service;
using Microsoft.JSInterop;
using System.Text;

namespace FabricCutter.UI.ViewModel
{
    public class RecipeViewModel : IRecipeViewModel
	{
		
		private readonly IToastService _toastService;
		private readonly IJSRuntime _jSRuntime;
		private readonly IEventHub _eventHub;
		private object _JsModule;

		public IRecipe _recipe { get; }
		public Action StateHasChanged { get; set; }= () => { };
		


		public RecipeViewModel(
			IToastService toastService, 
			IJSRuntime jSRuntime,
			IRecipe recipe,
			IEventHub eventHub
			)
        {   
			_toastService = toastService;
			_jSRuntime = jSRuntime;
			_recipe = recipe;
			_eventHub = eventHub;
			
			InitializeSubscribeEvents();

		}

		private void InitializeSubscribeEvents()
		{
			_eventHub.Subscribe(ApplicationEvents.OnExportedRecipe, OnExportedRecipeHandler);
		}
		private void UnsubscribeEvents()
		{
			_eventHub.Unsubscribe(ApplicationEvents.OnExportedRecipe, OnExportedRecipeHandler);
		}



		private async void OnExportedRecipeHandler(ApplicationEvents applicationEvents, object value)
		{
			try
			{
				var list = EventArgsAdapter.GetEventArgs<MarkerExportedEventArgs>(applicationEvents, value).markersList;
				var json = _recipe.RecipeDetailJsonFactory.Create(list);
				var fileName = "recipe.json";				
				var contnet = Encoding.UTF8.GetBytes(json);
				var fileStream = new MemoryStream(contnet);
				using var streamRef = new DotNetStreamReference(stream: fileStream);
				await _jSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
			}
			catch (Exception ex)
			{
				_toastService.ShowError($"Errore durante l'esportazione della ricetta: ");
                await Console.Out.WriteLineAsync(ex.InnerException+ ex.StackTrace);
                return;
			}
			
			_toastService.ShowSuccess($"Ricetta esportata");
			
		}


		public void Dispose()
		{
			UnsubscribeEvents();
		}
	}
}
