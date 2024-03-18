namespace FabricCutter.UI.Logic
{
	public interface IRecipeDetailJsonFactory
	{
		string Create(List<IMarkerBase> markerList);
	}
}