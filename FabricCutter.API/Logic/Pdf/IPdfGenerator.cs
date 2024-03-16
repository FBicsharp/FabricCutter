namespace FabricCutter.API.Logic.Pdf
{
	public interface IPdfGenerator
	{
		byte[] GeneratePdfAndRetriveByte(List<string> inputString);	
		Task<byte[]> GeneratePdfAndRetriveByteAsync(List<string> inputString);
	}
}