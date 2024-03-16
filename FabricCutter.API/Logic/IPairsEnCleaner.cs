namespace FabricCutter.API.Logic
{
	public interface IPairsEnCleaner
	{
		/// <summary>
		/// Remove the external letter if matching
		/// </summary>
		/// <param name="strings"></param>
		/// <returns></returns>
		IEnumerable<string> ProcessString(IEnumerable<string> inputString, string[] patterns = null);
		/// <summary>
		/// Remove the external letter if matching
		/// </summary>
		/// <param name="strings"></param>
		/// <returns></returns>
		Task<IEnumerable<string>> ProcessStringAsync(IEnumerable<string> strings, string[] patterns = null);
		/// <summary>
		/// Remove the external letter if matching
		/// </summary>
		/// <param name="inputString"></param>
		/// <param name="pattern"></param>
		/// <returns></returns>
		string RemoveExtenalCharacter(string inputString, string pattern);


	}
}