using System.Net.Http.Json;

namespace FabricCutter.UI.Service
{
	public class  ApplicationSettings
	{
		public UrlBaseSettings UrlBaseSettings { get; set; }
		public int SliderLenght { get; set; }

	}
	public class UrlBaseSettings 
    {
        public int SecondTimeout { get; set; }
        public string BaseAddress { get; set; }= "http://localhost:33500/";
        
		


	}
}
