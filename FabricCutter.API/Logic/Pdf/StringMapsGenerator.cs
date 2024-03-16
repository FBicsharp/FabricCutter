using System.Text;

namespace FabricCutter.API.Logic.Pdf
{
	public record CharMap(char Symbol, bool IsRounded, RoundedType RoundedType, short OrientationDegree);

	public class StringMapsGenerator : IStringMapsGenerator
	{
		const short NextStringOffset = 1;
		int x_Max { get; set; }
		int y_Max { get; set; }
		int x_currposition { get; set; }
		int y_currposition { get; set; }

		public CharMap[,] Maps { get; set; }

		public void Initialize(int widthMax_, int heightMax_)
		{
			x_Max = widthMax_ + NextStringOffset;
			y_Max = heightMax_ + NextStringOffset;
			Maps = new CharMap[x_Max, y_Max];
			for (int y = 0; y < y_Max; y++)
			{
				for (int x = 0; x < x_Max; x++)
				{
					Maps[y, x] = new CharMap(' ', false, RoundedType.None, 0);
				}
			}
		}

		private void MapContent(string s1, FaceSide Faceside, bool isRounded)
		{
			var firstNotSpace = s1.IndexOf(s1.Trim()[0]);
			for (int i = 0; i < s1.Length; i++)
			{
				var orientationDegree = (short)(((int)Faceside) * 90);
				var roundedType = RoundedType.None;
				if (s1[i]!=' ' )//is extra space added for do not writhing the text in the corners
				{
					if (isRounded && i == firstNotSpace)
						roundedType = RoundedType.Starting;
					else if (isRounded && i == s1.Length - 1)
						roundedType = RoundedType.Ending;
					else if (isRounded)
						roundedType = RoundedType.Central;
					else
						roundedType = RoundedType.None;
				}

				var c1 = new CharMap(s1[i], isRounded, roundedType, orientationDegree);			
				Maps[y_currposition, x_currposition] = c1;
				switch (Faceside)//calculate the next position step
				{
					case FaceSide.Top:
						x_currposition++;
						break;
					case FaceSide.Right:
						y_currposition++;
						break;
					case FaceSide.Bottom:
						x_currposition--;
						break;
					case FaceSide.Left:
						y_currposition--;
						break;
				}
			}
		}

		public string GetStringContent()
		{
			var sb = new StringBuilder();

			for (int y = 0; y < y_Max; y++)
			{
				for (int x = 0; x < x_Max; x++)
				{
					sb.Append(Maps[y, x].Symbol);
					Console.Write(Maps[y, x].Symbol);
				}
				sb.Append("\n");
			}
			return sb.ToString();
		}


		public CharMap[,] Generate(List<string> inputString)
		{
			var orderedInput = inputString
				.Where(s=>!string.IsNullOrWhiteSpace(s))//Clenaing empty rows
				.OrderByDescending(x => x.Length) //Ordering by length
				.ToList();
			var faceSide = FaceSide.Top; //Starting from top

			for (int i = 0; i < orderedInput.Count(); i++)
			{
				var currentStringLength = orderedInput[i].Trim().Length;
				var isRounded = false;
				var numRows = orderedInput.Count();
				if (i > 0 && i + 1 < numRows)
				{
					var prevStringLength = orderedInput[i - 1].Trim().Length;
					var nextStringLength = orderedInput[i + 1].Trim().Length;
					isRounded = currentStringLength == prevStringLength || currentStringLength == nextStringLength;
				}
				else if (i == 0 && i + 1 < numRows)
				{
					var nextStringLength = orderedInput[i + 1].Trim().Length;
					isRounded = currentStringLength == nextStringLength;
				}
				else if (i == numRows - 1 && i>0)
				{
					var prevStringLength = orderedInput[i - 1].Trim().Length;
					isRounded = currentStringLength == prevStringLength;
				}
				MapContent(orderedInput[i], faceSide, isRounded);
				faceSide++;
				if (faceSide > FaceSide.Left)
					faceSide = FaceSide.Top;
			}
			return Maps;
		}


	}


}

