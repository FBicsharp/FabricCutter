using FabricCutter.UI.Components;
using System.Collections.Generic;

namespace FabricCutter.UI.Logic
{
	public class RecipeInformation : IRecipeInformation
	{

		public int TotalLenght { get; private set; }
		public int MarkersLenght { get; private set; }
		public int SplicesNumber { get; private set; }
		public int MarkersNumber { get; private set; }
		public int AbsolutePosition { get; private set; }

		public List<Marker> Markers => _slider.Markers;
		readonly ISlider _slider;

		public RecipeInformation(ISlider slider)
		{

			_slider = slider;
			EvalutateMarkers();
		}

		private void EvalutateMarkers()
		{
			var cnt = Markers.Count();
			var tmpLenght = cnt > 0 ? (Markers.Max(m => m.EndPosition) - Markers.Min(m => m.StartPosition)) : 0;
			TotalLenght = tmpLenght < 0 ? 0 : tmpLenght;
			MarkersLenght = cnt > 0 ? Markers.Sum(m => m.MarkerLenght) : 0;
			SplicesNumber = cnt;
			MarkersNumber = cnt;
			AbsolutePosition = _slider.PointerPosition;//non se parte da sx o dx
		}


	}
}
