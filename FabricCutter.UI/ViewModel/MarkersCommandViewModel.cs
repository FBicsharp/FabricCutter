using Blazored.Toast.Services;
using FabricCutter.UI.Components;
using FabricCutter.UI.Logic;
using FabricCutter.UI.Logic.Events;
using FabricCutter.UI.Service;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace FabricCutter.UI.ViewModel
{

    /// <summary>
    /// Rappresenta le infiormazioni relative alla ricetta 
    /// </summary>
    public class MarkersCommandViewModel : IMarkersCommandViewModel
    {

        public bool IsStartMarkerEnable { get; private set; }
        public bool IsEndMarkerEnable { get; private set; }
        public bool IsStartSubMarkerEnable { get; private set; }
        public bool IsEndSubMarkerEnable { get; private set; }


        public Marker? MarkerInEditingMode { get; private set; }
        public Marker? ClosestMarkerFromCurretnPosition { get; private set; }
        public Marker? SecondMarkerInCurrentPosition { get; private set; }

        private int PointerPosition { get; set; }
        private List<Marker> Markers { get; set; }
        public Action StateHasChanged { get; set; } = () => { };

        private readonly IEventHub _eventHub;
        readonly IMarkerFactory _markerFactory;
        private readonly IToastService toastService;

        public MarkersCommandViewModel(
            IEventHub eventHub, IMarkerFactory markerFactory, IToastService toastService
            )
        {
            _eventHub = eventHub;
            Markers = new();
            _markerFactory = markerFactory;
            this.toastService = toastService;
            IsStartMarkerEnable = true;
            InitializeSubscribeEvents();

        }


        #region EVENTS




        private void InitializeSubscribeEvents()
        {
            _eventHub.Subscribe(ApplicationEvents.OnPointerPositionChanged, OnPointerPostionChangeHandler);
            _eventHub.Subscribe(ApplicationEvents.OnMarkerAdded, OnMarkerAddedHandler);
            _eventHub.Subscribe(ApplicationEvents.OnMarkerUpdated, OnMarkerUpdatedHandler);
            _eventHub.Subscribe(ApplicationEvents.OnResetMarker, OnResetMarkerHandler);
        }
        private void UnsubscribeEvents()
        {
            _eventHub.Unsubscribe(ApplicationEvents.OnPointerPositionChanged, OnPointerPostionChangeHandler);
            _eventHub.Unsubscribe(ApplicationEvents.OnMarkerAdded, OnMarkerAddedHandler);
            _eventHub.Unsubscribe(ApplicationEvents.OnMarkerUpdated, OnMarkerUpdatedHandler);
            _eventHub.Unsubscribe(ApplicationEvents.OnResetMarker, OnResetMarkerHandler);
        }



        private void OnMarkerAddedHandler(ApplicationEvents applicationEvents, object value)
        {
            Markers = EventArgsAdapter.GetEventArgs<MarkerAddedEventArgs>(applicationEvents, value).markersList;
            EvalutePossibleAction();
        }
        private void OnMarkerUpdatedHandler(ApplicationEvents applicationEvents, object value)
        {
            Markers = EventArgsAdapter.GetEventArgs<MarkerUpdatedEventArgs>(applicationEvents, value).markersList;
            EvalutePossibleAction();
        }



        private void OnResetMarkerHandler(ApplicationEvents applicationEvents, object value)
        {
            _markerFactory.Clear();
            Markers.Clear();
            MarkerInEditingMode = null;
            EvalutePossibleAction();
        }

        private void OnPointerPostionChangeHandler(ApplicationEvents applicationEvents, object value)
        {
            PointerPosition = EventArgsAdapter.GetEventArgs<PointerPositionChangedEventArgs>(applicationEvents, value).pointerPosition;
            EvalutePossibleAction();
        }

        #endregion


        public void StartMarker()
        {

            var newId = Markers.Count > 0 ? Markers.Max(m => m.Id) + 1 : 1;
            _markerFactory.WithStartMarkerPosition(newId, PointerPosition);
            if (MarkerInEditingMode is not null)
            {
                MarkerInEditingMode.StartPosition = PointerPosition;
                var update_args = new MarkerUpdateEventArgs(MarkerInEditingMode);
                _eventHub.Publish(ApplicationEvents.OnUpdateMarker, update_args);
                return;
            }
            MarkerInEditingMode = _markerFactory.BuildMarker();
            var args = new MarkerAddEventArgs(MarkerInEditingMode);
            _eventHub.Publish(ApplicationEvents.OnAddMarker, args);
            var args2 = new PointerPositionStartOrEndEventArgs(true, false);
            _eventHub.Publish(ApplicationEvents.OnPointerPositionStartOrEndChanged, args2);

        }

        public void EndMarker()
        {
            if (MarkerInEditingMode is null)
            {
                toastService.ShowWarning("Marcatore non trovato");
                return;
            }
            _markerFactory.WithStopMarkerPosition(PointerPosition);
            MarkerInEditingMode = _markerFactory.BuildMarker();
            if (MarkerInEditingMode.EndPosition == int.MinValue)
            {
                toastService.ShowWarning("La fine deve essere dopo l'inizio");
                return;
            }
            var args = new MarkerUpdateEventArgs(MarkerInEditingMode);
            _eventHub.Publish(ApplicationEvents.OnUpdateMarker, args);
            MarkerInEditingMode = null;
            _markerFactory.Clear();
            var args2 = new PointerPositionStartOrEndEventArgs(false, true);
            _eventHub.Publish(ApplicationEvents.OnPointerPositionStartOrEndChanged, args2);
        }


        public void StartSubMarker()
        {
            if (MarkerInEditingMode is null)
                return;
            _markerFactory.WithStartSubMarkerPosition(PointerPosition);
            MarkerInEditingMode = _markerFactory.BuildMarker();
            var args = new MarkerUpdateEventArgs(MarkerInEditingMode);
            _eventHub.Publish(ApplicationEvents.OnUpdateMarker, args);
        }
        public void EndSubMarker()
        {
            if (MarkerInEditingMode is null)
            {
                toastService.ShowWarning("Marcatore non trovato");
                return;
            }
            _markerFactory.WithStopSubMarkerPosition(PointerPosition);

            if (MarkerInEditingMode.SubMarker.Where(s => s.EndPosition == int.MinValue).Any())
            {
                toastService.ShowWarning("La fine deve essere dopo l'inizio");
                return;
            }
            MarkerInEditingMode = _markerFactory.BuildMarker();
            var args = new MarkerUpdateEventArgs(MarkerInEditingMode);
            _eventHub.Publish(ApplicationEvents.OnUpdateMarker, args);
        }

        #region BUTTON CHECKS


        public void EvalutePossibleAction()
        {
            IsStartSubMarkerEnable = false;
            IsEndSubMarkerEnable = false;
            IsStartMarkerEnable = false;
            IsEndMarkerEnable = false;
            DebugFlowMarker.Clear();
            DebugFlowSubMarker.Clear();
            CheckMarkerAction();
            CheckSubMarkerAction();
#if DEBUG
            Console.Clear();
            Console.WriteLine(DebugFlowMarker.ToString());
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine(DebugFlowSubMarker.ToString());

#endif

            StateHasChanged();
        }
        StringBuilder DebugFlowSubMarker = new();
        StringBuilder DebugFlowMarker = new();



        private IMarkerBase GetClosestMarker(int currentPosition)
        {
            int closestDistance = int.MaxValue;
            IMarkerBase closestMarker = new MarkerInvalid();
            foreach (var marker in Markers.OrderBy(m => m.Id))
            {
                int distance = Math.Abs(currentPosition - marker.StartPosition);
                if (distance < closestDistance)
                {
                    closestMarker = marker;
                    closestDistance = Math.Min(closestDistance, distance);
                }
            }
            return closestMarker;
        }






        private void CheckMarkerAction()
        {
            IsStartMarkerEnable = false;
            IsEndMarkerEnable = false;

            var closestMarker = GetClosestMarker(PointerPosition);
            ClosestMarkerFromCurretnPosition = closestMarker as Marker;

            if (closestMarker is MarkerInvalid || ClosestMarkerFromCurretnPosition is null)
            {
                DebugFlowMarker.AppendLine("D1| essite il nodo + vicino? NO");
                DebugFlowMarker.AppendLine("\t A1| startMarker enable");
                DebugFlowMarker.AppendLine("\t A1| EXIT ");
                IsStartMarkerEnable = true;
                StateHasChanged();
                return;
            }
            DebugFlowMarker.AppendLine("D1| essite il nodo + vicino? SI");

            var isInEditing = MarkerInEditingMode is not null;
            #region END MARKER COMMAND

            
            if (isInEditing)//controllo per la chiusura 
            {
                DebugFlowMarker.AppendLine("D2| sono in creazione marker? SI");
                var isClosed = MarkerInEditingMode.EndPosition != int.MinValue;
                DebugFlowMarker.AppendLine($"D3| La fine è valorizzata? {(isClosed ? "SI" : "No")}");
                if (isClosed)//ho già chiuso il marker?
                {
                    DebugFlowMarker.AppendLine($"\t A3| Start e stop disabilitati");
                    return;
                }

                var isOvelappedWithOtherMarker = Markers.Count(m => m.Id != MarkerInEditingMode.Id
                                                    && PointerPosition >m.StartPosition 
                                                    && PointerPosition <=m.EndPosition ) > 0;
                DebugFlowMarker.AppendLine($"D4| La fine è sovrapposta con  altri marker? {(isOvelappedWithOtherMarker ? "SI" : "No")}");
                if (isOvelappedWithOtherMarker)
                {
                    DebugFlowMarker.AppendLine($"\t A3| Start e stop disabilitati");
                    return;
                }

                var isAnySubMarkerNotClosed = MarkerInEditingMode.SubMarker.Any(s => s.EndPosition == int.MinValue);
                DebugFlowMarker.AppendLine($"D5| Tutti i sotto marcatori sono chiusi ? {(isAnySubMarkerNotClosed ? "SI" : "No")}");
                if (isAnySubMarkerNotClosed)
                {
                    DebugFlowMarker.AppendLine($"\t A3| Start e stop disabilitati");
                    return;
                }

                var isEndGreaterThanSubMarkerEnd = MarkerInEditingMode.SubMarker.Any() && MarkerInEditingMode.SubMarker.Max(s => s.EndPosition) <= PointerPosition;
                DebugFlowMarker.AppendLine($"D6| La fine è maggiore dell'ultimo sotto marcatore? {(isEndGreaterThanSubMarkerEnd && MarkerInEditingMode.SubMarker.Any() ? "SI" : "No")}");
                if (!isEndGreaterThanSubMarkerEnd && MarkerInEditingMode.SubMarker.Any())
                {
                    DebugFlowMarker.AppendLine($"\t A3| Start e stop disabilitati");
                    return;
                }
                //verifico che la distanza con altri marcatori sia di almeno 50
                var isMinimumDistanceRespected = Markers.Any() && Markers.Count(m => m.Id != MarkerInEditingMode.Id
                                                                    && (m.StartPosition <= PointerPosition)
                                                                    && Math.Abs(m.StartPosition - PointerPosition) < MarkerInEditingMode.Offset) == 0;
                DebugFlowMarker.AppendLine($"D7| la distanza tra inizio di altri marcatori è rispettata e lo stop è > di start? {(isMinimumDistanceRespected ? "SI" : "No")}");
                if (isMinimumDistanceRespected
                    && (PointerPosition > MarkerInEditingMode.StartPosition)
                )
                {
                    DebugFlowMarker.AppendLine($"\t A7| Stop abilitati");
                    IsEndMarkerEnable = true;
                    return;
                }
            }

            #endregion

            DebugFlowMarker.AppendLine("D2| sono in creazione marker? NO");


            #region START NEW MARKER

            var MaxMarkerOverlapped = 2;
            

            // trovo il marcatore con start e stop in cui la posizione corrente si trova
            var containerMarker = Markers
                .Where(m=>m.Id  != MarkerInEditingMode?.Id
                    && m.StartPosition <= PointerPosition
                    && m.EndPosition >= PointerPosition
                ).OrderBy(i=>i.Id).FirstOrDefault();
            DebugFlowMarker.AppendLine($"D8|{containerMarker?.Id }");
            DebugFlowMarker.AppendLine($"D8|{containerMarker?.StartPosition}");

            //controllo se questo containerMarker ha già altri marcatori che iniziano e finiscono tra start e stop
            var isCurrentPositionInOtherMarker = Markers.Count(m => m.Id != containerMarker?.Id 
                                                                 && m.Id != MarkerInEditingMode?.Id
                                                            && m.StartPosition >= containerMarker?.StartPosition) > 0;
            

            DebugFlowMarker.AppendLine($"D8| la posizione corrente sitrova tra start e stop di altri marcatori ? {(isCurrentPositionInOtherMarker ? "SI" : "No")}");
            if (isCurrentPositionInOtherMarker)
            {
                DebugFlowMarker.AppendLine($"\t A3| Start e stop disabilitati");
                return;
            }                       
            
            var isMinimumDistanceRespectedForStart = (PointerPosition- ClosestMarkerFromCurretnPosition.StartPosition ) < ClosestMarkerFromCurretnPosition.Offset;

            DebugFlowMarker.AppendLine($"D9| la distanza tra inizio di altri marcatori è rispettata? {(isMinimumDistanceRespectedForStart ? "SI" : "No")}");
            if (!isMinimumDistanceRespectedForStart)
            {
                DebugFlowMarker.AppendLine($"\t A9| Stop abilitato");
                IsStartMarkerEnable = true;
                return;
            }

            #endregion

        }
        private void CheckSubMarkerAction()
        {
            IsStartSubMarkerEnable = false;
            IsEndSubMarkerEnable = false;

            var isInEditing = MarkerInEditingMode is not null;
            if (!isInEditing)
                return;

            var hasSubMarkers = MarkerInEditingMode?.SubMarker is not null;
            if (!hasSubMarkers)
            {
                var isMinimumDistanceRespectedWithCurrent = (PointerPosition > MarkerInEditingMode?.StartPosition + 50)
                    && MarkerInEditingMode?.EndPosition == int.MinValue ?
                                            (PointerPosition < MarkerInEditingMode.EndPosition) :
                                            true;
                if (!isMinimumDistanceRespectedWithCurrent)
                    return;
                IsStartSubMarkerEnable = true;
                return;
            }

            var isClosed = MarkerInEditingMode.SubMarker.LastOrDefault()?.EndPosition != int.MinValue;
            if (isClosed)//ho già chiuso il marker?
                return;
            //TODO
            //var hasOverlappedWithOtherSubMarker = Markers.Count(m => 
            //			m.SubMarker is not null 
            //			&& m.SubMarker.Where(s=>s.EndPosition).EndPosition != int.MinValue //sono già chiusi
            //			&& m.SubMarker.Id != MarkerInEditingMode.SubMarker.Id
            //			&& PointerPosition >=m.SubMarker.StartPosition 
            //			&& PointerPosition <=m.SubMarker.EndPosition ) > 0;
            //var isMinimumDistanceRespected = Markers.Count(m => m.Id != MarkerInEditingMode.Id
            //										&& m.StartPosition <= PointerPosition
            //										&& m.EndPosition >= PointerPosition
            //										&& Math.Abs(m.StartPosition - PointerPosition) < 50) == 0;
            //if ((hasOverlappedWithOtherSubMarker && !isMinimumDistanceRespected) || !isMinimumDistanceRespected)
            //	return;
            IsEndSubMarkerEnable = true;

        }






        #endregion

        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }

}
