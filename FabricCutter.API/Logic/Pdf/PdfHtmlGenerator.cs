using System.Text;



namespace FabricCutter.API.Logic.Pdf
{
    public class PdfHtmlGenerator : IPdfHtmlGenerator
    {
        private readonly ILogger<PdfHtmlGenerator> logger;

        public PdfHtmlGenerator(ILogger<PdfHtmlGenerator> logger)
        {
            this.logger = logger;
        }
		public string GenerateHTMLTableFromMatirx(CharMap[,] matrix)
		{
			int numRows = matrix.GetLength(0);
			int numCols = matrix.GetLength(1);

			StringBuilder htmlTable = new StringBuilder();
			htmlTable.AppendLine("<!DOCTYPE html>");
			htmlTable.AppendLine("<html>");
			htmlTable.AppendLine("<head>");
			htmlTable.AppendLine("<style>");
			htmlTable.AppendLine("table { border-collapse: collapse;\r\n }\r\n");
			htmlTable.AppendLine("td {\r\nwidth: 30px;\r\nheight: 30px;\r\ntext-align: center;\r\nvertical-align: middle;\r\ntransform-origin: center center;\r\n \r\npadding: 7px 7px 7px 7px;\r\n}\r\n");

			htmlTable.AppendLine(".round-none-0 {\r\n border-style: none none none none;\r\n}\r\n");
			htmlTable.AppendLine(".round-starting-0 {\r\n border-style: dashed none dashed dashed  ;\r\n}\r\n");
			htmlTable.AppendLine(".round-central-0 {\r\n border-style: dashed none dashed none; \r\n}\r\n");
			htmlTable.AppendLine(".round-ending-0 {\r\n border-style: dashed dashed dashed none; \r\n}\r\n");

			htmlTable.AppendLine(".round-none-90 {\r\n border-style: none none none none;\r\n}\r\n");
			htmlTable.AppendLine(".round-starting-90 {\r\n border-style: dashed dashed none dashed;\r\n}\r\n");
			htmlTable.AppendLine(".round-central-90 {\r\n border-style: none dashed none dashed ; \r\n}\r\n");
			htmlTable.AppendLine(".round-ending-90 {\r\n border-style: none dashed dashed dashed; \r\n}\r\n");

			htmlTable.AppendLine(".round-none-180 {\r\n border-style: none none none none;\r\n}\r\n");
			htmlTable.AppendLine(".round-starting-180 {\r\n border-style: dashed dashed dashed none;\r\n}\r\n");
			htmlTable.AppendLine(".round-central-180 {\r\n border-style: dashed none dashed none; \r\n}\r\n");
			htmlTable.AppendLine(".round-ending-180 {\r\n border-style: dashed none dashed dashed; \r\n}\r\n");

			htmlTable.AppendLine(".round-none-270 {\r\n border-style: none none none none;\r\n}\r\n");
			htmlTable.AppendLine(".round-starting-270 {\r\n border-style: none dashed dashed dashed;\r\n}\r\n");
			htmlTable.AppendLine(".round-central-270 {\r\n border-style: none dashed none dashed; \r\n}\r\n");
			htmlTable.AppendLine(".round-ending-270{\r\n border-style: dashed dashed none dashed; \r\n}\r\n");

			htmlTable.AppendLine(".rotate-90 {\r\ntransform: rotate(90deg);\r\n}\r\n");
			htmlTable.AppendLine(".rotate-180 {\r\ntransform: rotate(180deg);\r\n}\r\n");
			htmlTable.AppendLine(".rotate-270 {\r\ntransform: rotate(270deg);\r\n}\r\n");
			htmlTable.AppendLine(".rounded {\r\nborder-style: dashed none dashed none;\r\n}\r\n");
			htmlTable.AppendLine("body { font-family: Verdana, sans-serif;\r\n    margin: 0;\r\n    padding: 0;\r\n    height: 100%;\r\n    display: flex;\r\n    justify-content: center;\r\n    align-items: center;\r\n}\r\n\r\n.center-container {\r\n    text-align: center;\r\n}");
			htmlTable.AppendLine("</style>");
			htmlTable.AppendLine("</head>");
			htmlTable.AppendLine("<body>");
			htmlTable.AppendLine("<div class=\"center-container\">");
			
			htmlTable.AppendLine("<table>");

			for (int row = 0; row < numRows; row++)
			{
				htmlTable.AppendLine("<tr>");

				for (int col = 0; col < numCols; col++)
				{	
					string styles = $"class=\" ";
					if (matrix[row, col].OrientationDegree > 0)
						styles += $"rotate-{matrix[row, col].OrientationDegree} ";
					
					if (matrix[row, col].IsRounded)//dipende anche dal lato in cui sono
						styles += $"round-{matrix[row, col].RoundedType.ToString().ToLower()}-{matrix[row, col].OrientationDegree} ";
					styles+="\"";

					htmlTable.AppendFormat($"<td {styles} >{matrix[row, col].Symbol}</td>");					
				}
				htmlTable.AppendLine("</tr>");
			}

			htmlTable.AppendLine("</table>");
			htmlTable.AppendLine("<div/>");
			htmlTable.AppendLine("</body>");
			htmlTable.AppendLine("</html>");

			return htmlTable.ToString();
		}

	}
}

