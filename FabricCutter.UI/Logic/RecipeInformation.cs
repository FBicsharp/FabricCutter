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

		public List<Marker> Markers { get; private set; }

		

		public void EvalutateMarkers(List<Marker> markers)
		{
			Markers = markers;
			var cnt = Markers.Count();
			var tmpLenght = cnt > 0 ? (Markers.Max(m => m.StartPosition) - Markers.Min(m => m.EndPosition)) : 0;
			TotalLenght = tmpLenght < 0 ? 0 : tmpLenght;
			MarkersLenght = cnt > 0 ? Markers.Sum(m => m.MarkerLenght) : 0;
			SplicesNumber = cnt;
			MarkersNumber = cnt;
			AbsolutePosition = 0;//non se parte da sx o dx
		}


	}
}
