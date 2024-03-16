using FabricCutter.API.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FabricCutter.API.Test
{
	public class PairsEnCleanerTest
	{
		private PairsEnCleaner _pairsEnCleaner;
		private string _brackets;

        public PairsEnCleanerTest()
        {
			var loggerMock = Substitute.For<ILogger<PairsEnCleaner>>();
			var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();

			_pairsEnCleaner = new PairsEnCleaner(loggerMock, httpContextAccessorMock);
        }

        [Theory]
		[InlineData("man", "a")]
		[InlineData("keep", "ee")]
		[InlineData("gqwertyuioplkjhgfdsazxcvbnm:?t", "qwertyuioplkjhgfdsazxcvbnm:?")]
		[InlineData("abcdefghijklmnopqrstuvwxyz", "")]
		
		public void ShouldCleanString(string inputStrings, string expectedResults)
		{
			// Arrange
			//is allready done in constructor

			// Act
			var ListOfStrings = new List<string>(){ inputStrings};
			var result = _pairsEnCleaner.ProcessString(ListOfStrings).First();

			// Assert
			Assert.Equal(result, expectedResults);
		}
	}
}