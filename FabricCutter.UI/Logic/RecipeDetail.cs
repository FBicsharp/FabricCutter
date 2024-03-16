using FabricCutter.UI.Components;
using System.Collections.Generic;

namespace FabricCutter.UI.Logic
{

	/// <summary>
	/// Rappresenta le infiormazioni relative alla ricetta 
	/// </summary>
	public class RecipeDetail
	{

		public string Name { get;  set; }
		public DateTime CreationDate { get;  set; }

		public int TotalLenght { get;  set; }
		public int MarkersLenght { get;  set; }
		public int SplicesNumber { get;  set; }
		public int MarkersNumber { get;  set; }
		public int AbsolutePosition { get;  set; }

		public List<Marker> Markers { get;  set; }

	

	


	}
}
