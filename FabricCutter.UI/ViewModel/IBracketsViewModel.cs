namespace FabricCutter.UI.ViewModel
{
    public interface IBracketsViewModel
	{
		string CurrentString { get; set; }
		Action StateHasChenged { get; set; }
		Task AddBracketsStringAsync();
        Task ProcessBracketsStringAsync();
        List<string> GetBracketsRequestString();
        List<string> GetBracketsResponseString();
		void ClearAll();

	}
}