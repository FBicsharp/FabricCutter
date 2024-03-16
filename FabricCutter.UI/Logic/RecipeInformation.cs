using FabricCutter.UI.Components;
using System.Collections.Generic;

namespace FabricCutter.UI.Logic
{

	/// <summary>
	/// Rappresenta le infiormazioni relative alla ricetta 
	/// </summary>
	public class RecipeInformation
	{

		public int TotalLenght { get; private set; }
		public int MarkersLenght { get; private set; }
		public int SplicesNumber { get; private set; }
		public int MarkersNumber { get; private set; }			
		public int AbsolutePosition { get; private set; }

		public  List<Marker> Markers { get; private set; }
		readonly Slider _slider;

		public RecipeInformation( Slider slider)
		{
			
			_slider = slider;
			EvalutateMarkers();
		}

		public void EvalutateMarkers()
		{
			Markers = _slider._markers;
			var cnt = Markers.Count();
			var tmpLenght = cnt > 0?(Markers.Max(m => m.EndPosition) - Markers.Min(m => m.StartPosition)):0;
			TotalLenght = tmpLenght < 0 ? 0 : tmpLenght;
			MarkersLenght = cnt > 0 ? Markers.Sum(m => m.MarkerLenght):0;			
			SplicesNumber = cnt;
			MarkersNumber = cnt;
			AbsolutePosition = _slider.PointerPosition;//non se parte da sx o dx
		}


	}
}
