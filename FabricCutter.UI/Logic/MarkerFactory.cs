namespace FabricCutter.UI.Logic
{
	public class MarkerFactory : IMarkerFactory
	{
		private Marker _currentMarker { get; set; }        


        public MarkerBase Build()
		{
			//_markerFactoryValidator.Validate(_currentMarker);
			//if (!_markerFactoryValidator.IsValid)
			//{
			//	return new MarkerInvalid();
			//}

			return _currentMarker;
		}

		public Marker WithStartMarkerPosition(int newMarkerId, int startPosition)
		{
			_currentMarker = new Marker(newMarkerId, startPosition, int.MinValue);
			return _currentMarker;
		}
		public Marker WithStopMarkerPosition(int endPosition)
		{
			if (_currentMarker is null)
				return _currentMarker;
			if (_currentMarker.EndPosition >= _currentMarker.StartPosition)
				return _currentMarker;
			_currentMarker.EndPosition = endPosition;
			return _currentMarker;
		}

		public Marker WithStartSubMarkerPosition(int startPosition)
		{
			if (_currentMarker is null || _currentMarker.SubMarker is null)
				return _currentMarker;
			_currentMarker.SubMarker = new SubMarker(_currentMarker.Id,startPosition, int.MinValue);
			return _currentMarker;
		}
		public Marker WithStopSubMarkerPosition(int endPosition)
		{
			if (_currentMarker is null || _currentMarker.SubMarker is null)
				return _currentMarker;

			if (_currentMarker.SubMarker.EndPosition <= _currentMarker.SubMarker.StartPosition)
				return _currentMarker;
			_currentMarker.EndPosition = endPosition;
			return _currentMarker;
		}



	}

	
}
