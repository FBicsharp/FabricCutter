using Newtonsoft.Json;

namespace FabricCutter.UI.Logic
{

    /// <summary>
    /// Generate JSON representation of a recipe detail.
    /// </summary>
    public class RecipeDetailJsonFactory : IRecipeDetailJsonFactory
    {
        public string Create(List<Marker> markerList)
        {
            return JsonConvert.SerializeObject(markerList, Formatting.Indented);
        }
    }
}
