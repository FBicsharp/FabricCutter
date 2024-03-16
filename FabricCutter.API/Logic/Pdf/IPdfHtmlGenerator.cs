namespace FabricCutter.API.Logic.Pdf
{
	public interface IPdfHtmlGenerator
	{
		/// <summary>
		/// Generate a HTML table from a matrix of CharMap
		/// </summary>
		/// <param name="matrix"></param>
		/// <returns>string as html content</returns>
		string GenerateHTMLTableFromMatirx(CharMap[,] matrix);		

	}
}