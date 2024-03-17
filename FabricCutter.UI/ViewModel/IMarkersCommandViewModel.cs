using Microsoft.AspNetCore.Components;

namespace FabricCutter.UI.ViewModel
{
    public interface IMarkersCommandViewModel 
    {
        bool IsStartMarkerEnable { get; }
        bool IsEndMarkerEnable { get; }
        bool IsEndSubMarkerEnable { get; }
        bool IsStartSubMarkerEnable { get; }

        void StartMarker();
        void EndMarker();
        void EndSubMarker();
        void StartSubMarker();


    }
}