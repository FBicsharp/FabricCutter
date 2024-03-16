using Blazored.Toast.Services;
using FabricCutter.UI.Service;
using Microsoft.JSInterop;

namespace FabricCutter.UI.ViewModel
{
    public class BracketsViewModel : IBracketsViewModel
	{
		private readonly IBracketsStringService _bracketsStringService;
		private readonly IToastService _toastService;
		private readonly IJSRuntime _jSRuntime;

		public string CurrentString { get; set; }

		private List<string> StringsList { get; set; }
		private List<string> StringsListResponse { get; set; }
        public Action StateHasChenged { get; set; }= () => { };
        public BracketsViewModel(IBracketsStringService bracketsStringService, IToastService toastService, IJSRuntime jSRuntime)
        {
            StringsList = new List<string>();
			StringsListResponse = new List<string>();
			CurrentString = string.Empty;
            _bracketsStringService = bracketsStringService;
			_toastService = toastService;
			_jSRuntime = jSRuntime;
		}

        public async Task AddBracketsStringAsync()
        {
			_toastService.ShowInfo($"Adding new item {CurrentString.Trim()}...");
			if (string.IsNullOrEmpty(CurrentString.Trim()))
				return;
			StringsList.Add(CurrentString);
            await ProcessBracketsStringAsync();
			StateHasChenged?.Invoke();
        }
        public async Task ProcessBracketsStringAsync()
        {
			_toastService.ShowInfo("Processing...");
			StringsListResponse = await _bracketsStringService.GetBracketsStringAsync(StringsList);            
            StateHasChenged?.Invoke();
        }
		public void ClearAll()
		{
			_toastService.ShowInfo("Clearing...");
			StringsList.Clear();
			StringsListResponse.Clear();
			StateHasChenged?.Invoke();
		}


		public List<string> GetBracketsRequestString() => StringsList;

        public List<string> GetBracketsResponseString() => StringsListResponse;

        
    }
}
