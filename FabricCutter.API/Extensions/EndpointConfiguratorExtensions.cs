using FabricCutter.API.Logic;
using FabricCutter.API.Logic.Pdf;

namespace FabricCutter.API.Extensions
{
	public static class EndpointConfiguratorExtensions
	{



		/// <summary>		
		/// Adds all endpoint to the application
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
		public static IApplicationBuilder MapAllEndpint(this WebApplication app)
		{
			
			app.MapPost("/cleanbrackets", async (List<string> inputString, HttpContext context) =>
			{
				var bracketsCleaner = context.RequestServices.GetRequiredService<IBracketsCleaner>();
				return await bracketsCleaner.ProcessStringAsync(inputString);
			})
			.Accepts<List<string>>("application/json")
			.Produces<List<string>>(StatusCodes.Status200OK);

			app.MapPost("/cleanpairs-en", async (List<string> inputString, HttpContext context) =>
			{
				var pairsEnCleaner = context.RequestServices.GetRequiredService<IPairsEnCleaner>();
				return await pairsEnCleaner.ProcessStringAsync(inputString);
			})
			.Accepts<List<string>>("application/json")
			.Produces<List<string>>(StatusCodes.Status200OK);

			app.MapPost("/topdf", async (List<string> inputString, HttpContext context) =>
			{
				var pdfGenerator = context.RequestServices.GetRequiredService<IPdfGenerator>();
				var PdfStream = await pdfGenerator.GeneratePdfAndRetriveByteAsync(inputString);
				var filename = "file.pdf";
				if (PdfStream.Length == 0)
					return Results.StatusCode(StatusCodes.Status204NoContent);
				return Results.File(PdfStream, "application/pdf", filename);

			})
			.Accepts<List<string>>("application/json")
			.Produces<byte[]>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status204NoContent);
			return app;
		}
	}
}
