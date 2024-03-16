using System.Text;

namespace FabricCutter.API.Logic.Pdf
{
	public interface IStringMapsGenerator
	{
		/// <summary>
		/// Matrix that rappresents the graphics  
		/// </summary>
		CharMap[,] Maps { get; set; }
		/// <summary>
		/// Return a rappresentation of the Maps matrix     
		/// </summary>
		/// <returns></returns>
		string GetStringContent();
		/// <summary>
		/// Process and inputString  to a Maps matrix
		/// </summary>
		/// <param name="inputString"></param>
		/// <returns></returns>
		CharMap[,] Generate(List<string> inputString);
		/// <summary>
		/// Initialize the Maps matrix
		/// </summary>
		/// <param name="widthMax_"></param>
		/// <param name="heightMax_"></param>
		void Initialize(int widthMax_, int heightMax_);
	}
}