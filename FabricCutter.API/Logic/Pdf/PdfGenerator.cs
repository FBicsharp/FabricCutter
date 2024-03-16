
//using BitMiracle.LibTiff.Classic;
//using System;
//using System.Drawing.Imaging;
//using System.Drawing.Printing;
//using WkHtmlToPdfDotNet;

using Wkhtmltopdf.NetCore;

namespace FabricCutter.API.Logic.Pdf
{
	public class PdfGenerator : IPdfGenerator
	{
		private readonly ILogger<PdfGenerator> _logger;
		private readonly IHttpContextAccessor _context;
		private readonly IPdfHtmlGenerator _htmlGenerator;
		private readonly IStringMapsGenerator _stringMapsGenerator;
		private readonly IGeneratePdf _generatePdf;

		public PdfGenerator(ILogger<PdfGenerator> logger, IHttpContextAccessor context, IPdfHtmlGenerator htmlGenerator, IStringMapsGenerator stringMapsGenerator/*, IGeneratePdf generatePdf*/)
		{
			_logger = logger;
			_context = context;
			_htmlGenerator = htmlGenerator;
			_stringMapsGenerator = stringMapsGenerator;
			/*_generatePdf = generatePdf;*/
		}

		public byte[] GeneratePdfAndRetriveByte(List<string> inputString)
		{
			var endpoint = _context.HttpContext?.Request?.Path;
			var ipAddress = _context.HttpContext?.Connection?.RemoteIpAddress;
			_logger.LogInformation($"Request to endpoint {endpoint} from IP {ipAddress}");
			var PdfStream = new byte[0];
			if (inputString.Count()==0)
				return PdfStream;
			try
			{
				_logger.LogDebug("Process string list");
				ResizeEqualStringLengths(ref inputString);
				var longhestString = inputString.Max(x => x.Length);
				_logger.LogInformation("Map list for graphic...");
				_stringMapsGenerator.Initialize(longhestString, longhestString);
				var html = _htmlGenerator.GenerateHTMLTableFromMatirx(_stringMapsGenerator.Generate(inputString));
				_logger.LogInformation("Generate PDF...");
				var Renderer = new IronPdf.ChromePdfRenderer().RenderHtmlAsPdf(html);
				PdfStream = Renderer.BinaryData;
				_logger.LogInformation("PDF generated");
				//PdfStream = _generatePdf.GetPDF(html);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during pdf generation");
			}

			return PdfStream;
		}
		public async Task<byte[]> GeneratePdfAndRetriveByteAsync(List<string> inputString)
			=> await Task.Factory.StartNew(() => GeneratePdfAndRetriveByte(inputString));

		/// <summary>
		/// Modify the length of strings by adding a space character to strings with equal lengths.
		/// </summary>
		/// <param name="inputStrings"></param>
		public void ResizeEqualStringLengths(ref List<string> inputStrings)
		{
			// Ordina la lista in modo crescente per lunghezza
			inputStrings = inputStrings.OrderBy(x => x.Length).ToList();
			var check = true;
			while (check)
			{
				check = false;
				for (int i = 0; i < inputStrings.Count; i++)
				{

					if ( i< inputStrings.Count-1 && inputStrings[i].Length >= inputStrings[i +1].Length)
					{
						// Aggiungi uno spazio all'inizio della stringa successiva
						inputStrings[i + 1] = " " + inputStrings[i+1];
						check = true;
					}
				}
			}
			inputStrings = inputStrings.OrderByDescending(x => x.Length).ToList();

		}

		

	}
}

