
namespace FabricCutter.UI.ViewModel
{
	public interface IViewModel : IDisposable
	{		
		Action StateHasChanged { get; set; }

	}
}