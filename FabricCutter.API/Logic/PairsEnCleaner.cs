
namespace FabricCutter.API.Logic
{
	public class PairsEnCleaner : IPairsEnCleaner
	{

		string[] defaultPatterns = {
			"az",
			"by",
			"cx",
			"dw",
			"ev",
			"fu",
			"gt",
			"hs",
			"ir",
			"jq",
			"kp",
			"lo",
			"mn"
		};
		private readonly ILogger<PairsEnCleaner> _logger;

		public IHttpContextAccessor _context { get; }

		public PairsEnCleaner(ILogger<PairsEnCleaner> logger, IHttpContextAccessor context)
		{
			_logger = logger;
			_context = context;
			var endpoint = _context.HttpContext.Request.Path;
			var ipAddress = _context.HttpContext.Connection.RemoteIpAddress;
			logger.LogInformation($"Request to endpoint {endpoint} from IP {ipAddress}\n ");
		}


		
		public IEnumerable<string> ProcessString(IEnumerable<string> inputString, string[] patterns = null)
		{

			if (patterns is null)
				patterns = defaultPatterns;//force default parameters
			var result = new List<string>();
			foreach (var s in inputString)
			{
				var tmpString= s;
				foreach (var pattern in patterns)
				{
					tmpString = RemoveExtenalCharacter(tmpString, pattern);
				}
				result.Add(tmpString);
			}

			return result;
		}
		
		public async Task<IEnumerable<string>> ProcessStringAsync(IEnumerable<string> strings, string[] patterns = null)
			=> await Task.FromResult(ProcessString(strings, patterns));
				
		public string RemoveExtenalCharacter(string inputString, string pattern)
		{
			var result = inputString;
			if (pattern.Length < 2)
				return result;


			if (result.Length >= 2 &&
				 result[0] == pattern[0] && result[result.Length - 1] == pattern[1]
				)
			{
				return result.Remove(result.Length - 1, 1).Remove(0, 1);
			}
			return result;
		}

	}


}

