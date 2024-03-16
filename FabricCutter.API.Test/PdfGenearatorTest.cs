using FabricCutter.API.Logic.Pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text;

namespace FabricCutter.API.Test
{
	public class PdfGenearatorTest
	{
		private PdfGenerator _pdfGenerator;

		public PdfGenearatorTest()
		{
			var loggerMock = Substitute.For<ILogger<PdfGenerator>>();

			var loggerPdfHtmlGenerator = Substitute.For<ILogger<PdfHtmlGenerator>>();
			var pdfHtmlGenerator = new PdfHtmlGenerator(loggerPdfHtmlGenerator) as IPdfHtmlGenerator;
			var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();

			var stringMapsGenerator = new StringMapsGenerator() as IStringMapsGenerator;
			_pdfGenerator = new PdfGenerator(loggerMock, httpContextAccessorMock, pdfHtmlGenerator, stringMapsGenerator);
		}

		[Theory]
		[InlineData(new string[] { "STAMPA1", "STAMPA2", "STAMPA3", "STAMPA4" },
					new string[] { "   STAMPA4", "  STAMPA3", " STAMPA2", "STAMPA1" }
		)]
		[InlineData(new string[] { "AAAA", "AAAA", "AAAA", "AAAA", "AAAA", "AAAA", "AAAA", "AAAA" },
					new string[] { "       AAAA", "      AAAA", "     AAAA", "    AAAA", "   AAAA", "  AAAA", " AAAA", "AAAA" }
		)]
		[InlineData(new string[] { "Ciao","bellissimo","marmellata","marmellata","carciofo","panevo2","Ciao","CIo","CI",
									"STAMPA4", "STAMPA2", "STAMPA2", "STAMPA1","STAMPA","STAMP" },
					new string[] { "      marmellata", "     marmellata", "    bellissimo", "     carciofo",
									"     STAMPA1","    STAMPA2","   STAMPA2","  STAMPA4"," panevo2"," STAMPA",
									" STAMP"," Ciao","Ciao","CIo","CI"}
		)]
		[InlineData(new string[] { "Ciao" }, new string[] { "Ciao" })]
		public void ShouldstretchStringIfLenghtIsduplicateString(string[] inputStrings, string[] expectedResults)
		{

			// Arrange
			//is allready done in constructor
			//Act			
			var list = inputStrings.ToList();
			_pdfGenerator.ResizeEqualStringLengths(ref list);
			// Assert
			Assert.Equal(list, expectedResults);
		}


		[Theory]
		[InlineData(new string[] { "STAMPA1", "STAMPA2", "STAMPA3", "STAMPA4" },true)]
		[InlineData(new string[] {  },false)]
		[InlineData(new string[] {  },false)]
		public void ShouldGeneratePdfFromStringList(string[] inputStrings,bool expectedResults)
		{

			// Arrange
			//is allready done in constructor
			//Act			
			var list = inputStrings.ToList();
			var bytes = _pdfGenerator.GeneratePdfAndRetriveByte(list);

			// Assert
			Assert.True(bytes is not null);
			Assert.Equal(expectedResults,(bytes.Count() > 0));
		}


	}
}