using FabricCutter.UI.Logic;
using Microsoft.AspNetCore.Components;

namespace FabricCutter.UI.ViewModel
{
    public interface ISliderViewModel : IViewModel
	{
		List<Marker> Markers { get; set; }
		int PointerPosition { get; set; }
		int SliderLenght { get; set; }

		void OnPositionPointerChange(ChangeEventArgs e);
		

	}
}