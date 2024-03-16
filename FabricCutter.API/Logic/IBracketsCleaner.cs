namespace FabricCutter.API.Logic
{
	public interface IBracketsCleaner
	{
		/// <summary>
		/// Remove extra brackets from string
		/// </summary>
		/// <param name="strings"></param>
		/// <returns></returns>
		IEnumerable<string> ProcessString(IEnumerable<string> inputString, string brackets = "()");

		Task<IEnumerable<string>> ProcessStringAsync(IEnumerable<string> strings, string brackets = "()");
		/// <summary>
		/// Remove the external brackets if they are extra until the string has balanced brackets
		/// </summary>
		/// <param name="inputString"></param>
		/// <param name="brackets"></param>
		/// <returns></returns>
		string RemoveExtenalExtraBrackets(string inputString, string brackets);

		/// <summary>
		/// Check if the brackets are balanced in the string and return true if they are balanced
		/// </summary>
		/// <param name="inputString"></param>
		/// <param name="brackets"></param>
		/// <returns></returns>
		bool IsBracketsBalanced(string inputString, string brackets);
	}
}