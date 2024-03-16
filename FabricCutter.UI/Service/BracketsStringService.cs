using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace FabricCutter.UI.Service
{
    public class BracketsStringService : IBracketsStringService
    {
        private readonly ILogger<BracketsStringService> _logger;
        private readonly HttpClient _httpClient;
		private readonly UrlBaseSettings _configurationService;

		public BracketsStringService(ILogger<BracketsStringService> logger, HttpClient httpClient, UrlBaseSettings configurationService)
        {
            _logger = logger;
            _httpClient = httpClient;
			_configurationService = configurationService;


		}
        public async Task<List<string>> GetBracketsStringAsync(List<string> inputString)
        {
            var result = new List<string>();
            try
            {
				var response = await _httpClient.PostAsJsonAsync("", inputString);
                if (response.IsSuccessStatusCode)
                {
					var responseContent = await response.Content.ReadAsStringAsync();
					result = JsonSerializer.Deserialize<List<string>>(responseContent);
				}
				else
                {
					_logger.LogError($"Error in GetBracketsStringAsync: {response.StatusCode}");
				}

            }
            catch (Exception ex)
            {
				_logger.LogError($"Error in GetBracketsStringAsync: {ex}");
            }
            result ??= new List<string>();
            return result;
        }


    }
}
